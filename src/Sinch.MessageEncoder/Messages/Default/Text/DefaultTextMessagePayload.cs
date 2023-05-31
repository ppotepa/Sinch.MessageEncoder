using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Extensions;
using System;
using System.Linq;
using System.Text;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    public class DefaultPayloadSerializer : Serialization.DefaultPayloadSerializer
    {
    }

    public class DefaultTextMessagePayload : Payload
    {
        [SerializeAs(typeof(string), Order = 1, Serializer = typeof(DefaultPayloadSerializer))]
        public string TextMessageBody { get; set; }

        public DefaultTextMessagePayload Deserialize(Span<byte> payloadSpan)
        {
            var result = new DefaultPayloadSerializer().Deserialize<DefaultTextMessagePayload>(payloadSpan);
            return result;
        }

        public override object Serialize()
        {
            byte[] textBytes = TextMessageBody.ToByteArray();
            return textBytes.Length.ToShortByteArray().Concat(textBytes).ToArray();
        }
    }
}