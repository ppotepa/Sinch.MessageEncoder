using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Factories.Serialization;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Sinch.MessageEncoder.Serialization;

namespace Sinch.MessageEncoder.Factories.Messages
{
    internal sealed class MessageFactory
    {
        private static readonly Dictionary<byte, Type> MessageTypes = default;
        private static readonly Dictionary<Type, Type> MessageTypesBinding = default;

        private static Dictionary<byte, Type> Factory(IEnumerable<Type> types) 
            => types.ToDictionary(type => type.GetMessageTypeCode(), type => type);

        static MessageFactory()
        {
            
            MessageTypes = AppDomain.CurrentDomain.GetSubclassesOf<Message, Dictionary<byte, Type>>(Factory);
            MessageTypesBinding = AppDomain.CurrentDomain.GetSubclassesOfOpenGeneric(typeof(Message<,>));
        }

        public static Message Create(byte[] messageBinary)
        {
            var serializers = GetSerializers
            (
                messageBinary: messageBinary, 
                targetBase: out var target, 
                messageTransport: out var messageTransport
            );

            return Activator.CreateInstance
            (
                type: MessageTypesBinding[typeof(Message<,>).MakeGenericType(target.GenericTypeArguments[0], target.GenericTypeArguments[1])],
                args: new object[]
                {
                    serializers.headers.Deserialize(target.GenericTypeArguments[0], messageTransport.HeaderTransportInfo),
                    serializers.payload.Deserialize(messageTransport.BinaryPayload, target.GenericTypeArguments[1])
                }
            )
            as Message;
        }

        private static (IHeadersSerializer headers, IPayloadSerializer payload) GetSerializers(byte[] messageBinary, 
            out Type targetBase, out MessageTransport messageTransport)
        {
            messageTransport = MessageTransport.FromSpan(messageBinary);
            targetBase = MessageTypes[messageTransport.HeaderTransportInfo.MSG_TYPE].BaseType;

            if (targetBase is { BaseType: null }) throw new InvalidOperationException("Message Type not supported");

            return (
                headers: SerializersFactory.CreateHeadersSerializer(targetBase.GenericTypeArguments[0]),
                payload: SerializersFactory.CreatePayloadSerializer(targetBase.GenericTypeArguments[1])
            );
        }

        public static byte[] Serialize(Message message)
        {
            (IHeadersSerializer headers, IPayloadSerializer payload) serializers = GetSerializers(message);

            List<byte> bytes = new List<byte>();

            bytes.AddRange(serializers.headers.Serialize(message.Headers as DefaultTextMessageHeaders));
            bytes.AddRange(serializers.payload.Serialize(message.Payload as DefaultTextMessagePayload));

            return bytes.ToArray();
        }

        private static (IHeadersSerializer headers, IPayloadSerializer payload) GetSerializers(Message message)
        {
            if (message.Headers is not MessageHeader headers) throw new InvalidOperationException();
            if (MessageTypes[headers.MessageType].BaseType is not { GenericTypeArguments.Length: 2 })
                throw new InvalidOperationException("Message type not supported.");

            Type targetBase = MessageTypes[headers.MessageType].BaseType;

            return (
                headers: SerializersFactory.CreateHeadersSerializer(targetBase.GenericTypeArguments[0]),
                payload: SerializersFactory.CreatePayloadSerializer(targetBase.GenericTypeArguments[1])
            );
        }
    }
}
