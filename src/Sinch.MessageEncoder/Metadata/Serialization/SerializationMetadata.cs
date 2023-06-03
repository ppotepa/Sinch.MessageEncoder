using System;
using Sinch.MessageEncoder.Attributes;
using System.Reflection;

namespace Sinch.MessageEncoder.Metadata.Serialization
{
    internal class SerializationMetadata
    {
        public SerializationMetadata(MessagePropertyAttribute attribute, PropertyInfo propertyInfo)
        {
            this.Attribute = attribute;
            this.PropertyInfo = propertyInfo;
        }

        public MessagePropertyAttribute Attribute { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        internal static SerializationMetadata Create(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            return new SerializationMetadata(
                property.GetCustomAttribute(typeof(MessagePropertyAttribute)) as MessagePropertyAttribute, property);
        }
    }
}
