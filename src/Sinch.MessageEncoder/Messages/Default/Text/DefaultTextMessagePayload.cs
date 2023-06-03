using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serialization.Default;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    [UseSerializer(typeof(DefaultPayloadSerializer))]
    public class DefaultTextMessagePayload : Payload
    {
        [MessageProperty(TargetType = typeof(string), Order = 1, PropertyName = nameof(TextMessageBody))]
        public string TextMessageBody { get; set; }

        [MessageProperty(TargetType = typeof(string), Order = 2, PropertyName = nameof(TextMessageBodyExtended))]
        public string TextMessageBodyExtended { get; set; }

        [MessageProperty(TargetType = typeof(int), Order = 2, PropertyName = nameof(Replies))]
        public int Replies { get; set; }
    }
}