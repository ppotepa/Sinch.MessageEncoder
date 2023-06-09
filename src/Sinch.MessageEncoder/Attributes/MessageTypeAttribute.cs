using System;

namespace Sinch.MessageEncoder.Attributes
{
    public class MessageTypeAttribute : Attribute
    {
        public byte MessageTypeCode { get; init; }

        public MessageTypeAttribute(byte messageTypeCode)
        {
            this.MessageTypeCode = messageTypeCode;
        }

        public MessageTypeAttribute()
        {
        }
    }
}
