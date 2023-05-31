using Sinch.MessageEncoder.Extensions;
using System;
using System.Linq;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    public class DefaultTextMessage : Message<DefaultTextMessagePayload>
    {
        object[] _serializationOrder;
        public DefaultTextMessage(MessageHeaderTransport headerTransport, Span<byte> payloadSpan) : base(headerTransport, payloadSpan)
        {
            int length = 0;
            int index = 0;
            Span<byte> current = default;

            do
            {
                short currentLength = BitConverter.ToInt16(payloadSpan[length..(length + 2)]);
                current = payloadSpan.Slice(length + 2, currentLength);
            }
            while ((length += 2 + current.Length) < payloadSpan.Length);

            Payload = new DefaultTextMessagePayload
            {
                SerializedText = System.Text.Encoding.Default.GetString(payloadSpan[2..payloadSpan.Length]),
            };
        }

        public override int HeadersCount => 3;
    }

    public class DefaultTextMessagePayload : Payload
    {   
        public string SerializedText { get; set; }
        protected override object[] SerializationOrder => new object[] { SerializedText };

        public override object Serialize()
        {
            byte[] textBytes = SerializedText.ToByteArray();
            return textBytes.Length
                .ToShortByteArray()
                .Concat(textBytes)
                .ToArray();
        }
    }
}
