using System.Linq;
using Sinch.MessageEncoder.Extensions;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class Payload
    {
        protected abstract object[] SerializationOrder { get; }

        public virtual object Serialize()
        {
            object[] order = SerializationOrder;
            var result = order.Select
            (
                property =>
                {
                    byte[] byteArr = property.ToByteArray();
                    return new[] { byteArr.Length.ToShortByteArray(), byteArr };
                }
            );

            return result.SelectMany(bytes => bytes);
        }
    }
}