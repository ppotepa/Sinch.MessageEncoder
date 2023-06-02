using System;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.Serialization
{
    public interface IPayloadSerializer
    {
        public TPayload Deserialize<TPayload>(Span<byte> payloadBytes) where TPayload : Payload, new();
        public Payload Deserialize(Span<byte> payloadBytes, Type payloadType);
        public byte[] Serialize<TPayload>(TPayload payload) where TPayload : Payload;
    }
}