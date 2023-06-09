﻿using Sinch.MessageEncoder.Messages;
using System;

namespace Sinch.MessageEncoder.Serializers
{
    public interface IPayloadSerializer : ISerializer
    {
        public Payload Deserialize(Type payloadType, ReadOnlySpan<byte> payloadSpan);
        public ReadOnlySpan<byte> Serialize<TPayload>(TPayload payload) where TPayload : Payload;
    }
}