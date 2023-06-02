using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serialization.Default;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    [UseSerializer(typeof(DefaultHeadersSerializer))]
    public class DefaultTextMessageHeaders : MessageHeader
    {
    }
}