using Sinch.MessageEncoder.Attributes;

namespace Sinch.MessageEncoder.CustomMessages.Tests.CustomMessages
{
    [MessageType(MessageTypeCode = 200)]
    internal class NotAMessage
    {
    }
    internal class NotAHeader
    {
    }

    internal class NotAPayload
    {
    }
}
