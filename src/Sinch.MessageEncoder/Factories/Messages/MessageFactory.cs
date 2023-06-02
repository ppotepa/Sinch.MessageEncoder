using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Factories.Serialization;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sinch.MessageEncoder.Factories.Messages
{
    internal sealed class MessageFactory
    {
        private static readonly Dictionary<byte, Type> MessageTypes = default;
        private static readonly Dictionary<Type, Type> MessageTypesBinding = default;

        static MessageFactory()
        {
            MessageTypes = AppDomain.CurrentDomain.GetSubclassesOf<Message, Dictionary<byte, Type>>
            (
                types => types.ToDictionary(type => type.GetCustomAttribute<MessageTypeAttribute>()!.MessageTypeCode, type => type)
            );
            MessageTypesBinding = AppDomain.CurrentDomain.GetSubclassesOfOpenGeneric(typeof(Message<,>));
        }

        public static Message Create(byte[] messageBinary)
        {
            MessageTransport messageTransport = MessageTransport.FromSpan(messageBinary);
            var target = MessageTypes[messageTransport.HeaderTransportInfo.MSG_TYPE];

            if (target.BaseType == null) 
                throw new InvalidOperationException("Message Type not supported");

            var headersType = target.BaseType.GenericTypeArguments[0];
            var payloadType = target.BaseType.GenericTypeArguments[1];

            var serializers = new
            {
                headers = SerializersFactory.CreateHeadersSerializer(headersType),
                payload = SerializersFactory.CreatePayloadSerializer(payloadType),
            };

            return Activator.CreateInstance
            (
                type: MessageTypesBinding[typeof(Message<,>).MakeGenericType(headersType, payloadType)],
                args: new object[]
                {
                    serializers.headers.Deserialize(headersType, messageTransport.HeaderTransportInfo),
                    serializers.payload.Deserialize(messageTransport.BinaryPayload, payloadType)
                }

            ) as Message;
        }

        public static byte[] Serialize(Message message)
        {
            if (message.Headers is not MessageHeader headers)
                throw new InvalidOperationException();

            if (MessageTypes[headers.MessageType].BaseType is not { GenericTypeArguments.Length: 2 }) 
                throw new InvalidOperationException("Message type not supported.");

            Type targetBase = MessageTypes[headers.MessageType].BaseType;

            var serializers = new
            {
                headers = SerializersFactory.CreateHeadersSerializer(targetBase.GenericTypeArguments[0]),
                payload = SerializersFactory.CreatePayloadSerializer(targetBase.GenericTypeArguments[1]),
            };
            
            return new[] {
                serializers.headers.Serialize(message.Headers as DefaultTextMessageHeaders),
                serializers.payload.Serialize(message.Payload as DefaultTextMessagePayload)
            }
            .SelectMany(bytes => bytes).ToArray();
        }
    }
}
