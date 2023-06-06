using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Transport;
using Sinch.MessageEncoder.Metadata.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sinch.MessageEncoder.Serializers.Default
{
    public class DefaultHeadersSerializer : IHeadersSerializer
    {
        private const int PropertyHeaderLength = 2;
        private static readonly Dictionary<Type, SerializationMetadata[]> HeadersMetadata = default;
        static DefaultHeadersSerializer()
        {
            HeadersMetadata = AppDomain.CurrentDomain
                .GetSubclassesOf<MessageHeader>()
                .ToDictionary(type => type, SerializationMetadata.Create);
        }

        public MessageHeader Deserialize(Type headersType, MessageHeaderTransport headersTransport)
        {
            int start = 0;

            if (Activator.CreateInstance(headersType)! is MessageHeader headers)
            {
                headers.Apply(headersTransport);
                var headerBytes = headersTransport.ADDITIONAL_HEADERS_BYTES;

                SerializationMetadata[] metadata = HeadersMetadata[headersType];

                foreach (var data in metadata)
                {
                    var currentPropertyLength = headerBytes.Slice(start, PropertyHeaderLength).ToInt16();
                    var currentHeaderBytes = headerBytes.Slice(start + PropertyHeaderLength, currentPropertyLength);

                    object @value = null;

                    if (data.PropertyInfo.PropertyType == typeof(string))
                    {
                        value = Encoding.ASCII.GetString(currentHeaderBytes);
                        data.PropertyInfo.SetValue(headers, Encoding.ASCII.GetString(currentHeaderBytes));
                    }

                    if (data.PropertyInfo.PropertyType == typeof(long))
                    {
                        value = currentHeaderBytes.ToInt64();
                        data.PropertyInfo.SetValue(headers, currentHeaderBytes.ToInt64());
                    }

                    if (data.PropertyInfo.PropertyType == typeof(int))
                    {
                        value = currentHeaderBytes.ToInt32();
                        data.PropertyInfo.SetValue(headers, currentHeaderBytes.ToInt32());
                    }

                    if (data.PropertyInfo.PropertyType == typeof(short))
                    {
                        value = currentHeaderBytes.ToInt16();
                        data.PropertyInfo.SetValue(headers, currentHeaderBytes.ToInt16());
                    }

                    if (data.PropertyInfo.PropertyType == typeof(byte))
                    {
                        value = currentHeaderBytes.ToInt8();
                        data.PropertyInfo.SetValue(headers, currentHeaderBytes.ToInt8());
                    }

                    if (value is not null)
                    {
                        headers[data.Attribute.HeaderName] = value;
                    }

                    start += PropertyHeaderLength + currentPropertyLength;
                }
            }
            else
                throw new InvalidOperationException($"{headersType.Name} is not valid Header Type.");

            return headers;
        }

        public ReadOnlySpan<byte> Serialize<THeaders>(THeaders headers)
            where THeaders : MessageHeader
        {
            return headers.DefaultBytes;
        }
    }
}