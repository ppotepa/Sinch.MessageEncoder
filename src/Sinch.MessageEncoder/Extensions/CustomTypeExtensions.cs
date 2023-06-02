using Sinch.MessageEncoder.Attributes;
using System;
using System.Reflection;

namespace Sinch.MessageEncoder.Extensions
{
    internal static class CustomTypeExtensions
    {
        public static Type ObtainSerializer(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return (type.GetCustomAttribute(typeof(UseSerializerAttribute)) as UseSerializerAttribute)?.Serializer;
        }
    }
}
