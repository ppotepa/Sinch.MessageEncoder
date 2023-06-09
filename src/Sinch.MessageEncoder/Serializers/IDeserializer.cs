using System;

namespace Sinch.MessageEncoder.Serializers;

public interface IDeserializer<out TPayload>
{
    public TPayload Deserialize(Span<byte> payloadSpan);
}