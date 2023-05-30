using System;
using System.Runtime.InteropServices;

namespace Sinch.MessageEncoder.Messages;

public ref struct MessageTransport
{
    public const int DEFAULT_UNCOMPRESSED_PAYLOADSIZE_TRANSPORT = 1024 * 1024 * 16;
    public Span<byte> BinaryPayload = default;
    public MessageHeaderTransport HeaderTransportInfo = new();

    private const int HEADERS_LENGTH_FIRST_BYTE_INDEX = 25;
    private const int LONG_LENGTH = 8;
    private const int MSG_TYPE_INDEX = 24;
    public MessageTransport()
    {
    }

    public static MessageTransport FromSpan(Span<byte> messageSpan)
    {
        long headersLength = MemoryMarshal.Read<long>(messageSpan[HEADERS_LENGTH_FIRST_BYTE_INDEX..(HEADERS_LENGTH_FIRST_BYTE_INDEX + 8)]);
        MessageTransport result = new()
        {
            HeaderTransportInfo = new MessageHeaderTransport
            {
                MSG_FROM = MemoryMarshal.Read<long>(messageSpan[..LONG_LENGTH]),
                MSG_TO = MemoryMarshal.Read<long>(messageSpan[LONG_LENGTH..(LONG_LENGTH * 2)]),
                MSG_TIMESTAMP = MemoryMarshal.Read<long>(messageSpan[(LONG_LENGTH * 2)..MSG_TYPE_INDEX]),
                MSG_TYPE = messageSpan[MSG_TYPE_INDEX],
                HEADERS_LENGTH = headersLength,
                //AdditionalHeaders = MessageHeader.ParseHeaders
            },

            BinaryPayload = messageSpan[(int)(25+8 + headersLength)..messageSpan.Length]
        };

        int payLoadStartByteIndex = MessageHeadersProcessor(messageSpan, ref result, headersLength);
        return result;
    }

    private static int MessageHeadersProcessor(Span<byte> messageSpan, ref MessageTransport transport, long headersLength)
    {
        Span<byte> allHeaders = messageSpan[(HEADERS_LENGTH_FIRST_BYTE_INDEX + 8) ..((int)headersLength + (HEADERS_LENGTH_FIRST_BYTE_INDEX + 8))];
        Span<byte> currentHeader = default;
        int index = default;

        for (index = 0; index < allHeaders.Length;)
        {
            short currentHeaderLength = BitConverter.ToInt16(allHeaders[index..(index + 2)]);
            currentHeader = allHeaders.Slice(index + 2, currentHeaderLength);
            
            if (currentHeaderLength > 0)
            {
                byte[] byteArray = new byte[currentHeaderLength];
                int indexRelative = index % 1024;

                transport.HeaderTransportInfo.AddHeader(currentHeader.ToArray());
                index += 2 + currentHeaderLength;
            }
            else break;
        }

        return index + currentHeader.Length;
    }
}