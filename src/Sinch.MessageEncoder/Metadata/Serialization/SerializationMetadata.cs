using Sinch.MessageEncoder.Attributes;
using System;
using System.Linq;
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

        internal static SerializationMetadata[] Create(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            PropertyInfo[] properties = type.GetProperties();

            return properties
                .Where(prop => prop.GetCustomAttribute<MessagePropertyAttribute>() is not null)
                .Select(prop =>
                {
                    var result = new SerializationMetadata(
                        prop.GetCustomAttribute(typeof(MessagePropertyAttribute)) as MessagePropertyAttribute, prop);
                    return result;
                })
                .OrderBy(data => data.Attribute.Order)
                .ToArray(); ;
        }
    }
}
