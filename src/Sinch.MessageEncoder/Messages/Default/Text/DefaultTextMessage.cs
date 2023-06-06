using Sinch.MessageEncoder.Attributes;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    [MessageType(MessageTypeCode = 1, Name = nameof(DefaultTextMessage))]
    public class DefaultTextMessage : Message<DefaultTextMessageHeaders, DefaultTextMessagePayload>
    {
        public override int HeadersCount => 1;

        public DefaultTextMessage(DefaultTextMessageHeaders headersFromTransports, DefaultTextMessagePayload payload) 
            : base(headersFromTransports, payload) { }

        public DefaultTextMessage() { }
    }
}
