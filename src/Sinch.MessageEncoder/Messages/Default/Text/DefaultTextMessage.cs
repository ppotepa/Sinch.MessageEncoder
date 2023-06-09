using Sinch.MessageEncoder.Attributes;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    [MessageType(MessageTypeCode = 1)]
    public class DefaultTextMessage : Message<DefaultTextMessageHeaders, DefaultTextMessagePayload>
    {
        public DefaultTextMessage(DefaultTextMessageHeaders headersFromTransports, DefaultTextMessagePayload payload)
            : base(headersFromTransports, payload)
        {
        }

        public DefaultTextMessage() { }
    }
}
