using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.CustomMessages.Tests.CustomMessages.InvalidOrder
{
    [MessageType(99)]
    internal class InvalidOrderMessage : Message<InvalidOrderHeader, InvalidOrderPayload>
    {
        public InvalidOrderMessage(InvalidOrderHeader headers, InvalidOrderPayload payload) : base(headers, payload)
        {
        }
    }
}
