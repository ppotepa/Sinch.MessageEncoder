using Sinch.MessageEncoder.Messages;
using System;

namespace Sinch.MessageEncoder.Serialization
{
    public interface IHeadersSerializer
    {
        public THeaders Deserialize<THeaders>(MessageHeaderTransport headersTransport) where THeaders : MessageHeader, new();
        public MessageHeader Deserialize(Type headersType, MessageHeaderTransport headersTransport);
        public byte[] Serialize<THeaders>(THeaders headers) where THeaders : MessageHeader;
    }
}