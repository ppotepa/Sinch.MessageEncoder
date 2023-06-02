using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Factories.Serialization;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using Sinch.MessageEncoder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sinch.MessageEncoder.Factories.Messages
{
    internal abstract class MessageFactory
    {
        private static readonly Dictionary<byte, Type> MessageTypes = AppDomain.CurrentDomain.GetSubclassesOf<Message, Dictionary<byte, Type>>
        (
            types => types.ToDictionary(type => type.GetCustomAttribute<MessageTypeAttribute>()!.MessageTypeCode, type => type)
        );

        private static Dictionary<Type, Type> MessageTypesBinding
            => AppDomain.CurrentDomain.GetSubclassesOfOpenGeneric(typeof(Message<,>));

        public static Message Create(byte[] messageBinary)
        {
            Message result = default;
            MessageTransport messageTransport = MessageTransport.FromSpan(messageBinary);

            if (messageTransport.HeaderTransportInfo.MSG_TYPE is 1)
            {
                var target = MessageTypes[1];

                if (target.BaseType != null)
                {
                    var headersType = target.BaseType.GenericTypeArguments[0];
                    var payloadType = target.BaseType.GenericTypeArguments[1];

                    IHeadersSerializer headersSerializer = HeadersSerializerFactory.CreateSerializer(headersType);
                    IPayloadSerializer payloadSerializer = PayloadSerializerFactory.CreateSerializer(payloadType);

                    var headers = headersSerializer.Deserialize(headersType, messageTransport.HeaderTransportInfo);
                    var payload = payloadSerializer.Deserialize(messageTransport.BinaryPayload, payloadType);
                    var targetGenericBaseType = typeof(Message<,>).MakeGenericType(headersType, payloadType);

                    result = Activator.CreateInstance(MessageTypesBinding[targetGenericBaseType], headers, payload) as Message;
                }
            }

            return result;
        }

        public static byte[] Serialize(Message message)
        {
            byte[] result = default;

            if (message is DefaultTextMessage)
            {
                IHeadersSerializer headersSerializer = HeadersSerializerFactory.CreateSerializer(typeof(DefaultTextMessageHeaders));
                IPayloadSerializer payloadSerializer = PayloadSerializerFactory.CreateSerializer(typeof(DefaultTextMessagePayload));

                var headers = headersSerializer.Serialize(message.Headers as DefaultTextMessageHeaders);
                var payload = payloadSerializer.Serialize(message.Payload as DefaultTextMessagePayload);

                result = new[] { headers, payload }.SelectMany(bytes => bytes).ToArray();
            }

            return result;
        }
    }
}
