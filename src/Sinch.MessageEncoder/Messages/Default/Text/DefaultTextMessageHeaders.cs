using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    [UseSerializer(typeof(DefaultHeadersSerializer))]
    public class DefaultTextMessageHeaders : MessageHeader
    {
        public DefaultTextMessageHeaders() { }
    }
}