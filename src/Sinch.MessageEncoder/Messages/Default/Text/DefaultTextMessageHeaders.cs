using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    [UseSerializer(typeof(DefaultHeadersSerializer))]
    public class DefaultTextMessageHeaders : MessageHeader
    {
        [SerializationOrder(Order = 1, HeaderName = "recipient-name")]
        public string SenderName { get; init; }

        [SerializationOrder(Order = 2, HeaderName = "sender-name")]
        public string RecipientName { get; set; }

        [SerializationOrder(Order = 3, HeaderName = "is-message-unread")]
        public bool IsMessageUnread { get; set; }
    }
}