﻿using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serialization.Default;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class Payload
    {
        public static Payload Empty => new EmptyPayload();
    }

    [UseSerializer(typeof(DefaultPayloadSerializer))]
    internal class EmptyPayload : Payload
    {

    }
}