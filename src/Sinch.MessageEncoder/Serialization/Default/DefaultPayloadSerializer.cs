using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Metadata.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sinch.MessageEncoder.Serialization.Default
{
    public class DefaultPayloadSerializer : IPayloadSerializer
    {
        private static readonly byte[] EmptyByteArray = { };
        private static readonly byte[] EmptyBytesResult = { 0, 0 };

        private static readonly Dictionary<Type, PropertyInfo[]> PayloadPropertiesMap = AppDomain.CurrentDomain
            .GetSubclassesOf<Payload>()
            .ToDictionary(type => type, type => type.GetProperties());

        public TPayload Deserialize<TPayload>(Span<byte> payloadSpan) where TPayload : Payload, new()
        {
            TPayload @new = new TPayload();

            var metadata = PayloadPropertiesMap[typeof(TPayload)].Select(property => new
            {
                Attribute = property.GetCustomAttribute(typeof(MessagePropertyAttribute)) as MessagePropertyAttribute,
                Property = property
            })
            .ToList();

            metadata.First().Property.SetValue(@new, Encoding.ASCII.GetString(payloadSpan[2..payloadSpan.Length]));
            return @new;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public Payload Deserialize(Span<byte> payloadSpan, Type payloadType)
        {
            IEnumerable<SerializationMetadata> metadata = payloadType.GetProperties().Select(SerializationMetadata.Create);
            
            object payload = Activator.CreateInstance(payloadType);

            DeserializeProperties(payloadSpan, metadata, payload);
            return payload as Payload;
        }

        public byte[] Serialize<TPayload>(TPayload payload) 
            where TPayload : Payload
        {
            List<byte> listBytes = new List<byte>();

            if (payload is null) 
                return EmptyBytesResult;

            var properties = PayloadPropertiesMap[payload.GetType()];
            var metadata = properties.Select(SerializationMetadata.Create);
            
            foreach (var data in metadata)
            {
                var array = data.PropertyInfo.GetValue(payload).ToByteArray();
                listBytes.AddRange((array.Length).ToByteArray());
                listBytes.AddRange(array);
            }

            return listBytes.ToArray();
        }

        private void DeserializeProperties(Span<byte> payloadBytes, IEnumerable<SerializationMetadata> metadata, object payload)
        {
            int start = 0;

            foreach (SerializationMetadata data in metadata)
            {
                var currentPropertyLength = payloadBytes.Slice(start, 4).ToInt32();
                var currentPropertyBytes = payloadBytes.Slice(start + 4, currentPropertyLength);

                if (data.Attribute.TargetType == typeof(string))
                    data.PropertyInfo.SetValue(payload, Encoding.ASCII.GetString(bytes: currentPropertyBytes));

                if (data.Attribute.TargetType == typeof(int))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToInt32());

                if (data.Attribute.TargetType == typeof(short))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToInt16());

                start += 4 + currentPropertyLength;
            }
        }
    }
}