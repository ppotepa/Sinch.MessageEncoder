namespace Sinch.MessageEncoder.Messages
{
    public class MessageHeader
    {
        public object AdditionalHeaders { get; internal set; }
        public long From { get; internal set; }
        public long HeadersLength { get; internal set; }
        public byte MessageType { get; internal set; }
        public long Timestamp { get; internal set; }
        public long To { get; internal set; }
    }
}