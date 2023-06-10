using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.MessageBuilder.Tests.CustomMessages.Default
{
    [MessageType(100)]
    internal class TestMessage : Message<TestMessageHeader, TestMessagePayload>
    {
        public TestMessage(TestMessageHeader headersFromTransports, TestMessagePayload payload)
            : base(headersFromTransports, payload) { }

        public TestMessage() { }

    }
}
