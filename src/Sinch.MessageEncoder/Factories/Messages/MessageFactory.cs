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
    public class MessageFactory
    {
        private static object _messageTypes = AppDomain
        .CurrentDomain.GetSubclassesOf<Message, Dictionary<byte, Type>>
        (
            (IEnumerable<Type> types) => types.ToDictionary(type => type.GetCustomAttribute<MessageTypeAttribute>()!.MessageTypeCode, 
            (Type type) => type)
        );

        public static Message Create(MessageTransport messageTransport)
        {
            Message result = default;

            if (messageTransport.HeaderTransportInfo.MSG_TYPE is 1)
            {
                IHeadersSerializer headersSerializer = HeadersSerializerFactory.CreateSerializer(typeof(DefaultTextMessageHeaders));
                IPayloadSerializer payloadSerializer = PayloadSerializerFactory.CreateSerializer(typeof(DefaultTextMessagePayload));

                DefaultTextMessageHeaders headers = headersSerializer.Deserialize<DefaultTextMessageHeaders>(messageTransport.HeaderTransportInfo);
                DefaultTextMessagePayload payload = payloadSerializer.Deserialize<DefaultTextMessagePayload>(messageTransport.BinaryPayload);

                result = new DefaultTextMessage
                {
                    Headers = headers,
                    Payload = payload
                };
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
