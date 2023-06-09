using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Builders;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class Payload : IBuildable
    {
    }

    [UseSerializer(typeof(DefaultPayloadSerializer))]
    internal class EmptyPayload : Payload
    {

    }
}