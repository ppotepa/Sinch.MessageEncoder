namespace Sinch.MessageEncoder.Messages.Default.Text;

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