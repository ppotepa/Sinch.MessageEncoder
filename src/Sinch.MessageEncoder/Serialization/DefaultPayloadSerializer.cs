using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sinch.MessageEncoder.Extensions;

namespace Sinch.MessageEncoder.Serialization
{
    internal interface IPayloadSerializer
    {
        public TPayload Deserialize<TPayload>(Span<byte> payload) where TPayload : Payload, new();

        public Span<byte> Serialize<TPayload>(TPayload payload) where TPayload : Payload;
    }

    public class DefaultPayloadSerializer : IPayloadSerializer
    {
        public TPayload Deserialize<TPayload>(Span<byte> payload) 
            where TPayload : Payload, new()
        {
            TPayload @new = new TPayload();

            var metadata = typeof(TPayload)
                .GetProperties()
                .Select(property => new
                {
                    Attribute = property.GetCustomAttribute(typeof(SerializeAsAttribute)) as SerializeAsAttribute,
                    Property = property
                })
                .OrderBy(pair => pair.Attribute.Order)
                .ToList();

            metadata.First().Property.SetValue(@new, Encoding.ASCII.GetString(payload[2..payload.Length]));

            return @new;
        }

        public Span<byte> Serialize<TPayload>(TPayload payload) where TPayload : Payload
        {
            return default;
        }
    }
}
