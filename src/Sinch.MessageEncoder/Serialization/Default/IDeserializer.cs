using System;

namespace Sinch.MessageEncoder.Serialization.Default;

public interface IDeserializer<out TPayload>
{
    public TPayload Deserialize(Span<byte> payloadSpan);
}