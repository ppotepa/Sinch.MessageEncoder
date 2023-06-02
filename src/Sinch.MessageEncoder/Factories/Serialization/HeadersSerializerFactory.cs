using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sinch.MessageEncoder.Factories.Serialization
{
    public static class HeadersSerializerFactory
    {
        internal static Dictionary<Type, Type> HeadersSerializers =
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(MessageHeader)) && type.IsAbstract is false)
                .ToDictionary(payload => payload,
                    payload => (payload.GetCustomAttribute(typeof(UseSerializerAttribute)) as UseSerializerAttribute)?.Serializer);


        public static IHeadersSerializer CreateSerializer<THeadersType>() where THeadersType : MessageHeader
        {
            return Activator.CreateInstance(HeadersSerializers[typeof(THeadersType)]) as IHeadersSerializer;
        }

        public static IHeadersSerializer CreateSerializer(Type headersType)
        {
            if (headersType == null) throw new ArgumentNullException(nameof(headersType));
            if (typeof(MessageHeader).IsAssignableFrom(headersType))
            {
                return Activator.CreateInstance(HeadersSerializers[headersType]) as IHeadersSerializer;
            }

            throw new ArgumentException($"{headersType.Name} is not assignable from {nameof(MessageHeader)}.");
        }
    }
}
