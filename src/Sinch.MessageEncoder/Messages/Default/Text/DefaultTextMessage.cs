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
                Name = System.Text.Encoding.Default.GetString(payloadSpan[..24]),
                Surname = System.Text.Encoding.Default.GetString(payloadSpan[24..48]),
                Text = System.Text.Encoding.Default.GetString(payloadSpan[48..72]),
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
            return new[] {
                new[]{(byte)Name.Length}.Concat(Name.ToByteArray()),
                new[]{(byte)Surname.Length}.Concat(Surname.ToByteArray()),
                new[]{(byte)Text.Length}.Concat(Text.ToByteArray()),
            }
            .SelectMany(bytes => bytes)
            .ToArray();
        }
    }
}
