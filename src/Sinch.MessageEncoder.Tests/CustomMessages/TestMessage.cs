using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.Tests.CustomMessages
{
    [MessageType(100, nameof(TestMessage))]
    internal class TestMessage : Message<TestMessageHeader, TestMessagePayload>
    {
    }

    internal class TestMessagePayload : Payload
    {
    }

    internal class TestMessageHeader : MessageHeader
    {
        [SerializationOrder(Order = 1, HeaderName = "byte")]
        public byte Byte { get; init; }

        [SerializationOrder(Order = 2, HeaderName = "short")]
        public short Short { get; set; }

        [SerializationOrder(Order = 3, HeaderName = "int")]
        public int Int { get; set; }

        [SerializationOrder(Order = 4, HeaderName = "long")]
        public long Long { get; set; }

        [SerializationOrder(Order = 5, HeaderName = "string")]
        public string String { get; set; }
    }
}
