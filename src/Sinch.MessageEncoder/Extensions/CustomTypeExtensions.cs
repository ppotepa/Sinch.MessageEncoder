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

        public static bool IsGenericTypeCandidate(this Type type, Type openGenericType)
        {
            return type.IsAbstract is false && type.IsInterface is false &&
                   type.BaseType is { IsGenericType: true } &&
                   type.BaseType.GetGenericTypeDefinition() == openGenericType;
        }

        public static byte GetMessageTypeCode(this Type type) =>
            type.GetCustomAttribute<MessageTypeAttribute>()!.MessageTypeCode;

    }

}