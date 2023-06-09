using System;

namespace Sinch.MessageEncoder.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class SerializationOrderAttribute : Attribute
    {
        public SerializationOrderAttribute()
        {
        }

        public int Order { get; set; }
        public string PropertyName { get; set; }
    }
}