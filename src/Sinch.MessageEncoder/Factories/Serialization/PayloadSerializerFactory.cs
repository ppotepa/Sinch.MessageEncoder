using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sinch.MessageEncoder.Factories.Serialization
{
    public static class PayloadSerializerFactory
    {
        internal static Dictionary<Type, Type> PayloadSerializers =
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Payload)) && type.IsAbstract is false)
                .ToDictionary(payload => payload, 
                    payload => (payload.GetCustomAttribute(typeof(UseSerializerAttribute)) as UseSerializerAttribute)?.Serializer);


        public static IPayloadSerializer CreateSerializer<TPayloadType>()
        {
            return Activator.CreateInstance(PayloadSerializers[typeof(TPayloadType)]) as IPayloadSerializer;
        }

        public static IPayloadSerializer CreateSerializer(Type payloadType)
        {
            if (payloadType == null) throw new ArgumentNullException(nameof(payloadType));

            if (typeof(Payload).IsAssignableFrom(payloadType))
            {
                return Activator.CreateInstance(PayloadSerializers[payloadType]) as IPayloadSerializer;
            }

            throw new ArgumentException($"{payloadType.Name} is not assignable from {nameof(Payload)}.");
        }
    }
}
