using System;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    public class DefaultTextMessage : Message<DefaultTextMessageHeaders, DefaultTextMessagePayload>
    {
        public DefaultTextMessage(MessageHeaderTransport headerTransport, Span<byte> payloadSpan) : base(headerTransport, payloadSpan)
        {
            Header = MessageHeader.FromTransport(headerTransport);
            Payload = Payload.Deserialize(payloadSpan);
        }

        public override int HeadersCount => Header.HeaderCount;
    }
}
