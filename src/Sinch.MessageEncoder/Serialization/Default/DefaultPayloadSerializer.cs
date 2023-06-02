using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sinch.MessageEncoder.Serialization.Default
{
    public class DefaultPayloadSerializer : IPayloadSerializer
    {
        public TPayload Deserialize<TPayload>(Span<byte> payloadBytes) where TPayload : Payload, new()
        {
            TPayload @new = new TPayload();

            var metadata = typeof(TPayload).GetProperties().Select(property => new
            {
                Attribute = property.GetCustomAttribute(typeof(MessagePropertyAttribute)) as MessagePropertyAttribute,
                Property = property
            })
            .ToList();

            metadata.First().Property.SetValue(@new, Encoding.ASCII.GetString(payloadBytes[2..payloadBytes.Length]));
            return @new;
        }

        public Payload Deserialize(Span<byte> payloadBytes, Type payloadType)
        {
            Payload @new = Activator.CreateInstance(payloadType) as Payload;
            var metadata = payloadType.GetProperties().Select(property => new
            {
                Attribute = property.GetCustomAttribute(typeof(MessagePropertyAttribute)) as MessagePropertyAttribute,
                Property = property
            });

            metadata.First().Property.SetValue(@new, Encoding.ASCII.GetString(payloadBytes[2..payloadBytes.Length]));
            return @new;
        }

        public byte[] Serialize<TPayload>(TPayload payload) where TPayload : Payload
        {
            if (payload is null)
            {
                return new[] { new byte[] { }.Length.ToShortByteArray(), new byte[] { } }.SelectMany(bytes => bytes).ToArray(); ;
            }
            else
            {
                var metadata = payload.GetType().GetProperties().Select(property => new
                {
                    Attribute = property.GetCustomAttribute(typeof(MessagePropertyAttribute)) as MessagePropertyAttribute,
                    Property = property
                })
                .Select(data =>
                {
                    var array = data.Property.GetValue(payload).ToByteArray();
                    var result = new[] { array.Length.ToShortByteArray(), array }.SelectMany(bytes => bytes);
                    return result;
                });

                return metadata.SelectMany(bytes => bytes).ToArray();
            }
        }
    }
}