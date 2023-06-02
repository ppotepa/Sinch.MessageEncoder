using System;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.Serialization
{
    public interface IPayloadSerializer
    {
        public TPayload Deserialize<TPayload>(Span<byte> payload) where TPayload : Payload, new();
        public byte[] Serialize<TPayload>(TPayload payload) where TPayload : Payload;
    }
}