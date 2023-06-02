using System;

namespace Sinch.MessageEncoder.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class UseSerializerAttribute : Attribute
    {
        public UseSerializerAttribute(Type serializer)
        {
            Serializer = serializer;
        }

        public Type Serializer { get; set; }
    }
}