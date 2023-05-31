namespace Sinch.MessageEncoder.Messages;

public class MessageHeader
{
    public readonly long From = default;
    public readonly long HeadersLength = default;
    public readonly byte MessageType = default;
    public readonly long Timestamp = default;
    public readonly long To = default;

    private readonly byte[] AdditionalHeaders = default;
    private byte _headerCount = 0;

    public MessageHeader()
    {
    }

    protected MessageHeader(long from, long to, byte messageType, long timestamp, long headersLength,
        byte[] additionalHeaders) 
    {
        From = from;
        To = to;
        MessageType = messageType;
        Timestamp = timestamp;
        HeadersLength = headersLength;
        AdditionalHeaders = additionalHeaders;
    }

    public byte HeaderCount
    {
        get
        {
            _headerCount = (byte)AdditionalHeaders.Length;
            return _headerCount;
        }
    }

    public static MessageHeader FromTransport(MessageHeaderTransport headerTransport)
    {
        return new MessageHeader
        (
            from: headerTransport.MSG_FROM,
            to: headerTransport.MSG_TO,
            messageType: headerTransport.MSG_TYPE,
            timestamp:
            headerTransport.MSG_TIMESTAMP,
            headersLength: headerTransport.HEADERS_LENGTH,
            additionalHeaders: headerTransport.HEADER_BYTES
        );
    }
}