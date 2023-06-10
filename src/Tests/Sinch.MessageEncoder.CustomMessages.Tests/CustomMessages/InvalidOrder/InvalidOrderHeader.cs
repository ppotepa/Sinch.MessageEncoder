﻿using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.CustomMessages.Tests.CustomMessages.InvalidOrder
{
    internal class InvalidOrderHeader : MessageHeader
    {
        [SerializationOrder(Order = 1, PropertyName = "header-1")]
        public string Header1 { get; init; }


        [SerializationOrder(Order = 1, PropertyName = "header-2")]
        public string Header2 { get; init; }

    }
}