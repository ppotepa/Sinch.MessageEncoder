using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Factories.Serialization
{
    internal static class SerializersFactory
    {
        internal static Dictionary<Type, Type> HeadersSerializers = default;
        internal static Dictionary<Type, Type> PayloadSerializers = default;

        static SerializersFactory()
        {
            HeadersSerializers = AppDomain.CurrentDomain
                .GetSubclassesOf<MessageHeader>()
                .ToDictionary(payload => payload, CustomTypeExtensions.ObtainSerializer);

            PayloadSerializers ??= AppDomain.CurrentDomain.GetSubclassesOf<Payload>()
                .ToDictionary(payload => payload, CustomTypeExtensions.ObtainSerializer);

        }

        public static IPayloadSerializer CreatePayloadSerializer(Type payloadType)
        {
            if (payloadType is null) 
                throw new ArgumentNullException(nameof(payloadType));

            return typeof(Payload).IsAssignableFrom(payloadType)
                ? Activator.CreateInstance(PayloadSerializers[payloadType]) as IPayloadSerializer
                : throw new ArgumentException($"{payloadType.Name} is not assignable from {nameof(Payload)}.");
        }

        public static IHeadersSerializer CreateHeadersSerializer(Type headersType)
        {
            if (headersType is null)
                throw new ArgumentNullException(nameof(headersType));

            return typeof(MessageHeader).IsAssignableFrom(headersType)
                ? Activator.CreateInstance(HeadersSerializers[headersType]) as IHeadersSerializer
                : throw new ArgumentException($"{headersType.Name} is not assignable from {nameof(MessageHeader)}.");
        }
    }
}
