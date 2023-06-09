using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.MessageBuilder.Tests.CustomMessages.Default
{
    internal class TestMessagePayload : Payload
    {
        [SerializationOrder(Order = 1, PropertyName = nameof(TestTextBody))]
        public string TestTextBody { get; set; }
    }
}