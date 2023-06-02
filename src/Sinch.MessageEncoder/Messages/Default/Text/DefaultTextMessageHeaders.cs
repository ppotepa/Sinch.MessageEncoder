using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serialization.Default;

namespace Sinch.MessageEncoder.Messages.Default.Text;

[UseSerializer(typeof(DefaultHeaderSerializer))]
public class DefaultTextMessageHeaders : MessageHeader
{
    public DefaultTextMessageHeaders()
    {
    }

    protected DefaultTextMessageHeaders(long from, long to, byte messageType, long timestamp, 
        long headersLength, byte[] additionalHeaders) 
        : base(from, to, messageType, timestamp, headersLength, additionalHeaders)
    {
    }
}