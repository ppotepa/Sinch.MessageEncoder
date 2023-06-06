using System;

namespace Sinch.MessageEncoder.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class SerializationOrderAttribute : Attribute
    {
        public SerializationOrderAttribute(int order, string headerName)
        {
            Order = order;
            HeaderName = headerName;
        }

        public SerializationOrderAttribute()
        {
        }

        public int Order { get; set; }
        public string HeaderName { get; set; }
    }
}