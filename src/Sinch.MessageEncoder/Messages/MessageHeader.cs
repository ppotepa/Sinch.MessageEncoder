namespace Sinch.MessageEncoder.Messages;

public class MessageHeader
{
    private byte _headerCount = 0;
    public MessageHeader()
    {
    }

    public object AdditionalHeaders { get; init; }
    public long From { get; init; }
    public long HeadersLength { get; init; }
    public byte MessageType { get; init; }
    public long Timestamp { get; init; }
    public long To { get; init; }
}