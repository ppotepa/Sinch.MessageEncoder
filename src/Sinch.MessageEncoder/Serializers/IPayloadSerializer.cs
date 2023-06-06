using System;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Serializers
{
    public interface IPayloadSerializer : ISerializer
    {
        public Payload Deserialize(Type payloadType, ReadOnlySpan<byte> payloadSpan);
        public ReadOnlySpan<byte> Serialize<TPayload>(TPayload payload) where TPayload : Payload;
    }
}