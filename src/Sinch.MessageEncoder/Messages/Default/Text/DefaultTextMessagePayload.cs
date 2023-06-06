using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    [UseSerializer(typeof(DefaultPayloadSerializer))]
    public class DefaultTextMessagePayload : Payload
    {
        [MessageProperty(Order = 1, PropertyName = nameof(TextMessageBody))]
        public string TextMessageBody { get; set; }
    }
}