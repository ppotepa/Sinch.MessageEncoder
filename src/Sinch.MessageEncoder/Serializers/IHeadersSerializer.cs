using System;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Serializers
{
    public interface IHeadersSerializer : ISerializer
    {
        public MessageHeader Deserialize(Type headersType, MessageHeaderTransport headersTransport);
        public ReadOnlySpan<byte> Serialize<THeaders>(THeaders headers) where THeaders : MessageHeader;
    }
}