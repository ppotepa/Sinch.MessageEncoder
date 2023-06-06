using System;

namespace Sinch.MessageEncoder.Serializers.Default;

public interface IDeserializer<out TPayload>
{
    public TPayload Deserialize(Span<byte> payloadSpan);
}