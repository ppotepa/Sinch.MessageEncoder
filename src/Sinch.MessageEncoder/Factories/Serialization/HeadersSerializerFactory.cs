using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Factories.Serialization
{
    public static class HeadersSerializerFactory
    {
        internal static Dictionary<Type, Type> _headersSerializers = default;
        internal static Dictionary<Type, Type> HeadersSerializers
        {
            get
            {
                if (_headersSerializers is null)
                {
                    _headersSerializers = AppDomain.CurrentDomain.GetSubclassesOf<MessageHeader>()
                        .ToDictionary(payload => payload, CustomTypeExtensions.ObtainSerializer);
                }
                return _headersSerializers;
            }
        }

        public static IHeadersSerializer CreateSerializer<THeadersType>() where THeadersType : MessageHeader
        {
            return Activator.CreateInstance(HeadersSerializers[typeof(THeadersType)]) as IHeadersSerializer;
        }

        public static IHeadersSerializer CreateSerializer(Type headersType)
        {
            if (headersType is null)
                throw new ArgumentNullException(nameof(headersType));

            if (typeof(MessageHeader).IsAssignableFrom(headersType))
            {
                return Activator.CreateInstance(HeadersSerializers[headersType]) as IHeadersSerializer;
            }

            throw new ArgumentException($"{headersType.Name} is not assignable from {nameof(MessageHeader)}.");
        }
    }
}
