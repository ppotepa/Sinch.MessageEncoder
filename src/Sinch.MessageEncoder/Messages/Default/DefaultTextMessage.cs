using System;
using System.Runtime.InteropServices;

namespace Sinch.MessageEncoder.Messages.Default
{
    public class DefaultTextMessage : Message<DefaultTextMessagePayload>
    {
        public DefaultTextMessage(MessageHeaderTransport transport, Span<byte> payloadSpan) : base(transport, payloadSpan)
        {
            this.Payload = new DefaultTextMessagePayload
            {
                Name = System.Text.Encoding.Default.GetString(payloadSpan[..24]),
                Surname = System.Text.Encoding.Default.GetString(payloadSpan[..24]),
            };
        }

        public override int HeadersCount => 3;
    }

    public class DefaultTextMessagePayload : IPayload
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
