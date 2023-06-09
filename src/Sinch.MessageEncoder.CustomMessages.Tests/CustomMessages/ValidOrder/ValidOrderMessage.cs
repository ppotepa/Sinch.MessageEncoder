using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.CustomMessages.Tests.CustomMessages.ValidOrder
{
    [MessageType(98)]
    internal class ValidOrderMessage : Message<ValidOrderHeader, ValidOrderPayload>
    {
        public ValidOrderMessage(ValidOrderHeader headers, ValidOrderPayload payload) : base(headers, payload)
        {
        }
    }
}
