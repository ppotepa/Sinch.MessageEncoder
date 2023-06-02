using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Object = System.Object;

namespace Sinch.MessageEncoder.Serialization.Default
{
    public class DefaultPayloadSerializer : IPayloadSerializer
    {
        public TPayload Deserialize<TPayload>(Span<byte> payload) where TPayload : Payload, new()
        {
            TPayload @new = new TPayload();

            var metadata = typeof(TPayload).GetProperties().Select(property => new
            {
                Attribute = property.GetCustomAttribute(typeof(SerializeAsAttribute)) as SerializeAsAttribute,
                Property = property
            })
            .ToList();

            metadata.First().Property.SetValue(@new, Encoding.ASCII.GetString(payload[2..payload.Length]));
            return @new;
        }

        public byte[] Serialize<TPayload>(TPayload payload) where TPayload : Payload
        {
            if (payload is null)
            {
                return new[] { new byte[]{}.Length.ToShortByteArray(), new byte[]{} }.SelectMany(bytes => bytes).ToArray();;
            }
            else
            {
                var metadata = payload.GetType().GetProperties().Select(property => new
                    {
                        Attribute = property.GetCustomAttribute(typeof(SerializeAsAttribute)) as SerializeAsAttribute,
                        Property = property
                    })
                    .ToList();

                var result = metadata.ToList().Select(data =>
                {
                    var array = data.Property.GetValue(payload).ToByteArray();
                    var result = new[] { array.Length.ToShortByteArray(), array }.SelectMany(bytes => bytes);
                    return result;
                }).ToArray();

                return result.SelectMany(bytes => bytes).ToArray();
            }
        }
    }
}