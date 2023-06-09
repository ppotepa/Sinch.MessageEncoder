﻿using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.Tests.CustomMessages.ValidOrder
{
    [MessageType(98)]
    internal class ValidOrderMessage : Message<ValidOrderHeader, ValidOrderPayload>
    {
        public ValidOrderMessage(ValidOrderHeader headers, ValidOrderPayload payload) : base(headers, payload)
        {
        }
    }

    internal class ValidOrderPayload : Payload
    {
    }

    internal class ValidOrderHeader : MessageHeader
    {
        [SerializationOrder(Order = 1, PropertyName = "header-1")]
        public string Header1 { get; init; }
    }
}
