using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using System.Linq;

namespace Sinch.MessageEncoder.Serialization.Default
{
    public class DefaultHeadersSerializer : IHeadersSerializer
    {
        public THeaders Deserialize<THeaders>(MessageHeaderTransport headersTransport) where THeaders : MessageHeader, new()
        {
            THeaders @new = new()
            {
                From = headersTransport.MSG_FROM,
                To = headersTransport.MSG_TO,
                MessageType = headersTransport.MSG_TYPE,
                Timestamp = headersTransport.MSG_TIMESTAMP,
                HeadersLength = headersTransport.HEADERS_LENGTH,
                AdditionalHeaders = headersTransport.HEADER_BYTES
            };

            return @new;
        }
        
        public byte[] Serialize<THeaders>(THeaders headers) where THeaders : MessageHeader
        {
            var from = headers.From.ToByteArray();
            var to = headers.To.ToByteArray();
            var stamp = headers.Timestamp.ToByteArray();
            var type = headers.MessageType.ToByteArray();
            var len = headers.HeadersLength.ToByteArray();
            var add = headers.AdditionalHeaders.ToByteArray();

            return new[] { from, to, stamp, type, len, add }.SelectMany(x => x).ToArray();
        }
    }
}