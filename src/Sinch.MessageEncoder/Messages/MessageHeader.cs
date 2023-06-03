using Sinch.MessageEncoder.Extensions;
using System.Linq;

namespace Sinch.MessageEncoder.Messages
{
    public class MessageHeader
    {
        public MessageHeader()
        {
        }

        public MessageHeader(MessageHeaderTransport headersTransport)
        {
            this.From = headersTransport.MSG_FROM;
            this.To = headersTransport.MSG_TO;
            this.MessageType = headersTransport.MSG_TYPE;
            this.Timestamp = headersTransport.MSG_TIMESTAMP;
            this.HeadersLength = headersTransport.HEADERS_LENGTH;
            this.AdditionalHeaders = headersTransport.HEADER_BYTES;
        }

        public object AdditionalHeaders { get; internal set; }
        public long From { get; internal set; }
        public long HeadersLength { get; internal set; }
        public byte MessageType { get; internal set; }
        public long Timestamp { get; internal set; }
        public long To { get; internal set; }

        internal byte[] DefaultBytes => new[]
        {
            From,
            To,
            Timestamp,
            MessageType,
            HeadersLength,
            AdditionalHeaders
        }
        .SelectMany(@object => @object.ToByteArray())
        .ToArray();
    }
}