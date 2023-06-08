using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Metadata.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sinch.MessageEncoder.Serializers.Default
{
    public class DefaultPayloadSerializer : IPayloadSerializer
    {
        private const int PropertyHeaderLength = 4;
        private static readonly byte[] EmptyBytesResult = { 0, 0 };

        private static readonly Dictionary<Type, SerializationMetadata[]> PayloadMetadata = AppDomain.CurrentDomain
                .GetSubclassesOf<Payload>()
                .ToDictionary(type => type, SerializationMetadata.Create);

        public Payload Deserialize(Type payloadType, ReadOnlySpan<byte> payloadSpan)
        {
            Payload payload = Activator.CreateInstance(payloadType) as Payload;
            
            if (payloadSpan.Length > 0)
            {
                DeserializeProperties(payloadSpan, payload);
            }

            return payload;
        }

        public ReadOnlySpan<byte> Serialize<TPayload>(TPayload payload)
            where TPayload : Payload
        {
            List<byte> listBytes = new List<byte>();

            if (payload is null)
                return EmptyBytesResult;

            var metadata = PayloadMetadata[payload.GetType()];

            foreach (var data in metadata)
            {
                var array = data.PropertyInfo.GetValue(payload).ToByteArray();
                listBytes.AddRange((array.Length).ToByteArray(), array);
            }

            return listBytes.ToArray();
        }

        private void DeserializeProperties(ReadOnlySpan<byte> payloadBytes, Payload payload)
        {
            Dictionary<Type, object> map = new Dictionary<Type, object>();

            int start = 0;

            IEnumerable<SerializationMetadata> metadata = PayloadMetadata[payload.GetType()];

            foreach (SerializationMetadata data in metadata)
            {
                var currentPropertyLength = payloadBytes.Slice(start, PropertyHeaderLength).ToInt32();
                var currentPropertyBytes = payloadBytes.Slice(start + PropertyHeaderLength, currentPropertyLength);

                if (data.PropertyInfo.PropertyType == typeof(string))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.GetString());

                if (data.PropertyInfo.PropertyType == typeof(long))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToInt64());

                if (data.PropertyInfo.PropertyType == typeof(long?))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToNullableInt64());

                if (data.PropertyInfo.PropertyType == typeof(int))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToInt32());

                if (data.PropertyInfo.PropertyType == typeof(int?))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToNullableInt32());

                if (data.PropertyInfo.PropertyType == typeof(short))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToInt16());

                if (data.PropertyInfo.PropertyType == typeof(short?))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToNullableInt16());

                if (data.PropertyInfo.PropertyType == typeof(byte))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToInt8());

                if (data.PropertyInfo.PropertyType == typeof(byte?))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToNullableInt8());

                if (data.PropertyInfo.PropertyType == typeof(float))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToSingle());

                if (data.PropertyInfo.PropertyType == typeof(float?))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToNullableSingle());

                if (data.PropertyInfo.PropertyType == typeof(double))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToDouble());

                if (data.PropertyInfo.PropertyType == typeof(double?))
                    data.PropertyInfo.SetValue(payload, currentPropertyBytes.ToNullableDouble());

                start += PropertyHeaderLength + currentPropertyLength;
            }
        }
    }
}