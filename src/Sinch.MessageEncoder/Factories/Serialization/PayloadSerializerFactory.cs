using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Factories.Serialization
{
    public static class PayloadSerializerFactory
    {
        internal static Dictionary<Type, Type> _payloadSerializers = default;

        internal static Dictionary<Type, Type> PayloadSerializers
        {
            get
            {
                if (_payloadSerializers is null)
                {
                    _payloadSerializers = AppDomain.CurrentDomain.GetSubclassesOf<Payload>()
                        .ToDictionary(payload => payload, CustomTypeExtensions.ObtainSerializer);
                }
                return _payloadSerializers;
            }
        }

        public static IPayloadSerializer CreateSerializer<TPayloadType>() where TPayloadType : Payload
        {
            return Activator.CreateInstance(PayloadSerializers[typeof(TPayloadType)]) as IPayloadSerializer;
        }

        public static IPayloadSerializer CreateSerializer(Type payloadType)
        {
            if (payloadType is null) throw new ArgumentNullException(nameof(payloadType));

            if (typeof(Payload).IsAssignableFrom(payloadType))
            {
                return Activator.CreateInstance(PayloadSerializers[payloadType]) as IPayloadSerializer;
            }

            throw new ArgumentException($"{payloadType.Name} is not assignable from {nameof(Payload)}.");
        }
    }
}
