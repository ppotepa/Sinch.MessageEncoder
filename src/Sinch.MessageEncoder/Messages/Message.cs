namespace Sinch.MessageEncoder.Messages
{
    public struct Message
    {
        public MessageHeader Header { get; init; }
        public byte[] Payload { get; init; }
    }

    public struct MessageTransport
    {
        public MessageHeaderTransport Header { get; init; }
        public byte[] Payload { get; init; }
    }
}
