using Sinch.MessageEncoder.Extensions;
using System;
using System.Linq;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    public class DefaultTextMessage : Message<DefaultTextMessagePayload>
    {
        public DefaultTextMessage(MessageHeaderTransport headerTransport, Span<byte> payloadSpan) : base(headerTransport, payloadSpan)
        {
            Payload = new DefaultTextMessagePayload
            {
                Name = System.Text.Encoding.Default.GetString(payloadSpan[2.. payloadSpan.Length]),
            };
        }

        public override int HeadersCount => 3;
    }

    public class DefaultTextMessagePayload : Payload
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Text { get; set; }

        public override object Serialize()
        {
            byte[] nameBytes = Name.ToByteArray();
            return ((short)nameBytes.Length).ToByteArray().Concat(nameBytes).ToArray();
        }
    }
}
