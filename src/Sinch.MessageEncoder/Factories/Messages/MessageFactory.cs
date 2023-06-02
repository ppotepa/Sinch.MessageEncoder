using System.Collections.Generic;
using Sinch.MessageEncoder.Factories.Serialization;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using Sinch.MessageEncoder.Serialization;

namespace Sinch.MessageEncoder.Factories.Messages
{
    public class MessageFactory
    {
        
        public static Message Create(MessageTransport messageTransport)
        {
            Message result = default;

            if (messageTransport.HeaderTransportInfo.MSG_TYPE is 1)
            {
                IPayloadSerializer serializer = PayloadSerializerFactory.CreateSerializer(typeof(DefaultTextMessagePayload));
                DefaultTextMessagePayload payload = serializer.Deserialize<DefaultTextMessagePayload>(messageTransport.BinaryPayload);
                result = new DefaultTextMessage
                {
                    Header = MessageHeader.FromTransport(messageTransport.HeaderTransportInfo),
                    Payload = payload
                };
            }

            return result;
        }

        public static Message Create(byte[] messageTransport)
        {
            return default;
        }
    }
}
