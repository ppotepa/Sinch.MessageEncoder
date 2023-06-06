using System;
using System.Net.Http;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.Serializers.Default
{
    public class DefaultHeadersSerializer : IHeadersSerializer
    {
        public MessageHeader Deserialize(Type headersType, MessageHeaderTransport headersTransport)
        {
            if (Activator.CreateInstance(headersType)! is MessageHeader headers)
            {
                headers.From = headersTransport.MSG_FROM;
                headers.To = headersTransport.MSG_TO;
                headers.MessageType = headersTransport.MSG_TYPE;
                headers.Timestamp = headersTransport.MSG_TIMESTAMP;
                headers.MessageType = headersTransport.MSG_TYPE;
                headers.HeadersLength = headersTransport.HEADERS_LENGTH;
                
                //if (headers.HeadersLength != 0)
                {
                    
                    var byteArray = new ReadOnlySpan<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                    headers.Map<MessageHeader, string>("123", x => x.MyTestObject);
                }
            }
            else
                throw new InvalidOperationException($"{headersType.Name} is not valid Header Type.");
            return headers;
        }

        public ReadOnlySpan<byte> Serialize<THeaders>(THeaders headers)
            where THeaders : MessageHeader
        {
            return headers.DefaultBytes;
        }
    }
}