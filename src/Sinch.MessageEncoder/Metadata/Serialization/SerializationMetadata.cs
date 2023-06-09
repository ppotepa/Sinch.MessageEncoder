using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace Sinch.MessageEncoder.Metadata.Serialization
{
    internal class SerializationMetadata
    {
        public SerializationMetadata(SerializationOrderAttribute attribute, PropertyInfo propertyInfo)
        {
            this.Attribute = attribute;
            this.PropertyInfo = propertyInfo;
        }

        public SerializationOrderAttribute Attribute { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        internal static SerializationMetadata[] Create(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            PropertyInfo[] properties = type.GetProperties();

            var propertiesWithAttributes = properties
                .Where(prop => prop.GetCustomAttribute<SerializationOrderAttribute>() is not null)
                .Select(prop =>
                {
                    var result = new SerializationMetadata(
                        prop.GetCustomAttribute(typeof(SerializationOrderAttribute)) as SerializationOrderAttribute,
                        prop);

                    return result;
                })
                .ToArray();

            var grouped = propertiesWithAttributes.GroupBy(prop => prop.Attribute.Order);
            var moreThanOnce = grouped.Where(group => group.Count() > 1).ToArray();

            if (moreThanOnce.Any())
            {
                var message = string.Join("", moreThanOnce.Select(x => x.Key));

                throw new InvalidSerializationOrderException(
                   $"Unable to build metadata. Some Property orders are duplicated. {message}"
                );
            }

            return propertiesWithAttributes;
        }
    }
}
