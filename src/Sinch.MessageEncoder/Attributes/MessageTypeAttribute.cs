using System;

namespace Sinch.MessageEncoder.Attributes
{
    public class MessageTypeAttribute : Attribute 
    {
        public readonly byte MessageCode;
        public readonly string Name;

        public MessageTypeAttribute(byte messageCode, string name)
        {
            this.MessageCode = messageCode;
            this.Name = name;
        }
    }
}
