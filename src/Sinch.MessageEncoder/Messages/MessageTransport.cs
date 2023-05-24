using System;
using System.Runtime.InteropServices;

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
        return new MessageTransport
        {
            HeaderTransportInfo = new MessageHeaderTransport
            {
                MSG_FROM = MemoryMarshal.Read<long>(messageSpan[..8]),
                MSG_TO = MemoryMarshal.Read<long>(messageSpan[8..16]),
                MSG_TIMESTAMP = MemoryMarshal.Read<long>(messageSpan[16..24]),
                MSG_TYPE = messageSpan[24]
            },

            BinaryPayload = messageSpan[24..messageSpan.Length]
        };
    }
}