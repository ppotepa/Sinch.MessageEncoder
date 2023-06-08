using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class Payload : IBuildable
    {
        public static Payload Empty => new EmptyPayload();
    }

    [UseSerializer(typeof(DefaultPayloadSerializer))]
    internal class EmptyPayload : Payload
    {

    }
}