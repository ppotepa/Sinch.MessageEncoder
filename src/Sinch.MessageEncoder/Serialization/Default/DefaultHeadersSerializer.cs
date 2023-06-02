using Sinch.MessageEncoder.Messages;
using System;

namespace Sinch.MessageEncoder.Serialization.Default
{
    public class DefaultHeadersSerializer : IHeadersSerializer
    {
        public THeaders Deserialize<THeaders>(MessageHeaderTransport headersTransport)
            where THeaders : MessageHeader, new()
            => Deserialize(typeof(THeaders), headersTransport) as THeaders;

        public MessageHeader Deserialize(Type headersType, MessageHeaderTransport headersTransport) 
            => Activator.CreateInstance(headersType, headersTransport) as MessageHeader;

        public byte[] Serialize<THeaders>(THeaders headers)
            where THeaders : MessageHeader => headers.DefaultBytes;
    }
}