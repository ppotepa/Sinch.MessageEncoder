using System;

namespace Sinch.MessageEncoder.Attributes
{
    public class MessageTypeAttribute : Attribute
    {
        public byte MessageTypeCode { get; init; }
        public string Name { get; init; }

        public MessageTypeAttribute(byte messageTypeCode, string name)
        {
            this.MessageTypeCode = messageTypeCode;
            this.Name = name;
        }

        public MessageTypeAttribute()
        {
        }
    }
}
