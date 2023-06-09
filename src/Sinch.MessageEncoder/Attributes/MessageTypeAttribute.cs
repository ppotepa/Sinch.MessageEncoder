﻿using System;

namespace Sinch.MessageEncoder.Attributes
{
    public class MessageTypeAttribute : Attribute
    {
        public MessageTypeAttribute(byte messageTypeCode)
        {
            this.MessageTypeCode = messageTypeCode;
        }

        public MessageTypeAttribute()
        {
        }

        public byte MessageTypeCode { get; init; }
    }
}
