using Sinch.MessageEncoder.Extensions;
using System;

namespace Sinch.MessageEncoder.Messages;

public ref struct MessageTransport
{
    public const int DEFAULT_UNCOMPRESSED_PAYLOADSIZE_TRANSPORT = 1024 * 1024 * 16;
    public Span<byte> BinaryPayload = default;
    public MessageHeaderTransport HeaderTransportInfo = new();

    public MessageTransport()
    {
    }

    public static MessageTransport FromSpan(Span<byte> messageSpan)
    {
        long headersLength = messageSpan.GetMessageHeadersLength();

        MessageTransport result = new()
        {
            HeaderTransportInfo = MessageHeaderTransport.FromSpan(messageSpan, headersLength),
            BinaryPayload = messageSpan[(int)(25 + 8 + headersLength)..messageSpan.Length]
        };

        int payLoadStartByteIndex = MessageHeadersProcessor(messageSpan, ref result, headersLength);
        return result;
    }

    private static int MessageHeadersProcessor(Span<byte> messageSpan, ref MessageTransport transport, long headersLength)
    {
        Span<byte> allHeaders = messageSpan.GetAllHeaders(headersLength);
        Span<byte> currentHeader = default;
        int index = default;

        for (index = 0; index < allHeaders.Length;)
        {
            short currentHeaderLength = BitConverter.ToInt16(allHeaders[index..(index + 2)]);
            currentHeader = allHeaders.Slice(index + 2, currentHeaderLength);

            if (currentHeaderLength > 0)
            {
                index += 2 + currentHeaderLength;
            }
            else break;
        }

        return index + currentHeader.Length;
    }
}