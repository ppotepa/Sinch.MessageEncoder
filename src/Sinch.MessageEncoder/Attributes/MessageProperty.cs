using System;

namespace Sinch.MessageEncoder.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class MessagePropertyAttribute : Attribute
    {
        public MessagePropertyAttribute(Type targetType, int order, string propertyName)
        {
            TargetType = targetType;
            Order = order;
            PropertyName = propertyName;
        }

        public MessagePropertyAttribute()
        {
        }

        public Type TargetType { get; set; }
        public int Order { get; set; }
        public string PropertyName { get; set; }
    }
}