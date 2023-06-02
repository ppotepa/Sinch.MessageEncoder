using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.Serialization
{
    public interface IHeadersSerializer
    {
        public THeaders Deserialize<THeaders>(MessageHeaderTransport headersTransport) where THeaders : MessageHeader, new();
        public byte[] Serialize<THeaders>(THeaders headers) where THeaders : MessageHeader;
    }
}