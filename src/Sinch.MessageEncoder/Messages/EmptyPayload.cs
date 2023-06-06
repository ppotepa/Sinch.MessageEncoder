using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Messages
{
    [UseSerializer(typeof(DefaultPayloadSerializer))]
    internal class EmptyPayload : Payload
    {

    }
}