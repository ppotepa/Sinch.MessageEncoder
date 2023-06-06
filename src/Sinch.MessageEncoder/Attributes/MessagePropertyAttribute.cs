using System;

namespace Sinch.MessageEncoder.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class MessagePropertyAttribute : Attribute
    {
        public MessagePropertyAttribute(int order, string propertyName)
        {
            Order = order;
            PropertyName = propertyName;
        }

        public MessagePropertyAttribute()
        {
        }

        public int Order { get; set; }
        public string PropertyName { get; set; }
    }
}