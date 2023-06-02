using System;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using System.Linq;

namespace Sinch.MessageEncoder.Serialization.Default
{
    public class DefaultHeadersSerializer : IHeadersSerializer
    {
        public THeaders Deserialize<THeaders>(MessageHeaderTransport headersTransport) 
            where THeaders : MessageHeader, new()
        {
            return Deserialize(typeof(THeaders), headersTransport) as THeaders;
        }

        public MessageHeader Deserialize(Type headersType, MessageHeaderTransport headersTransport)
        {
            var header = Activator.CreateInstance(headersType) as MessageHeader;

            header.From = headersTransport.MSG_FROM;
            header.To = headersTransport.MSG_TO;
            header.MessageType = headersTransport.MSG_TYPE;
            header.Timestamp = headersTransport.MSG_TIMESTAMP;
            header.HeadersLength = headersTransport.HEADERS_LENGTH;
            header.AdditionalHeaders = headersTransport.HEADER_BYTES;


            return header;
        }

        public byte[] Serialize<THeaders>(THeaders headers)
            where THeaders : MessageHeader
        {
            return new[]
            {
                headers.From,
                headers.To, 
                headers.Timestamp, 
                headers.MessageType,
                headers.HeadersLength,
                headers.AdditionalHeaders
            }
            .SelectMany(x => x.ToByteArray())
            .ToArray();
        }
    }
}