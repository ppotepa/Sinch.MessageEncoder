using Sinch.MessageEncoder.Extensions;
using System;
using System.Linq;
using System.Text;

namespace Sinch.MessageEncoder.Messages.Default.Text;

public class DefaultTextMessagePayload : Payload
{
    public string TextMessageBody { get; init; }

    protected override object[] SerializationOrder => new object[] { () => TextMessageBody };

    public DefaultTextMessagePayload Deserialize(Span<byte> payloadSpan)
    {
        SerializationOrder[0] = Encoding.ASCII.GetString(payloadSpan[2..payloadSpan.Length]);
        return this;
    }

    public override object Serialize()
    {
        var textBytes = TextMessageBody.ToByteArray();
        return textBytes.Length.ToShortByteArray().Concat(textBytes).ToArray();
    }
}