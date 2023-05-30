using System;
using System.Runtime.InteropServices;

namespace Sinch.MessageEncoder.Messages;

public ref struct MessageTransport
{
    public const int DEFAULT_UNCOMPRESSED_PAYLOADSIZE_TRANSPORT = 1024 * 1024 * 16;
    public Span<byte> BinaryPayload = default;
    public MessageHeaderTransport HeaderTransportInfo = new();

    private const int MSG_TYPE_INDEX = 24;
    private const int HEADERS_START_INDEX = 25;
    private const int LONG_LENGTH = 8;

    public MessageTransport()
    {
    }

    public static MessageTransport FromSpan(Span<byte> messageSpan)
    {
        MessageTransport result = new()
        {
            HeaderTransportInfo = new MessageHeaderTransport
            {
                MSG_FROM = MemoryMarshal.Read<long>(messageSpan[..LONG_LENGTH]),
                MSG_TO = MemoryMarshal.Read<long>(messageSpan[LONG_LENGTH..(LONG_LENGTH * 2)]),
                MSG_TIMESTAMP = MemoryMarshal.Read<long>(messageSpan[(LONG_LENGTH * 2)..MSG_TYPE_INDEX]),
                MSG_TYPE = messageSpan[MSG_TYPE_INDEX],
                PAYLOAD_START_INDEX = 9999,
                //AdditionalHeaders = MessageHeader.ParseHeaders
            },

            BinaryPayload = messageSpan[24..messageSpan.Length]
        };


        int payLoadStartByteIndex = MessageHeadersProcessor(messageSpan, ref result);
        return result;
    }

    private static int MessageHeadersProcessor(Span<byte> messageSpan, ref MessageTransport transport)
    {
        long payloadStartIndex = MemoryMarshal.Read<long>(messageSpan[25..(25 + 8)]);
        Span<byte> allHeaders = messageSpan[HEADERS_START_INDEX..(int)payloadStartIndex];
        
        int index = 0;
        for (index = 0; index < allHeaders.Length;)
        {
            short currentHeaderLength = BitConverter.ToInt16(allHeaders[index..(index + 2)]);

            if (currentHeaderLength > 0)
            {
                Span<byte> currentHeader = allHeaders[2..(currentHeaderLength + 2)];
                byte[] byteArray = new byte[currentHeaderLength];
                int indexRelative = index % 1024;

                transport.HeaderTransportInfo.AddHeader(currentHeader.ToArray());
                index += 2 + currentHeaderLength;
            }
            else break;
        }

        return index;
    }
}