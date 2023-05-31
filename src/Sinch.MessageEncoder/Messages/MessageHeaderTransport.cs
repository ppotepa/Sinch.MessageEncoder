using System;
using System.Collections.Generic;
using System.Linq;
using Sinch.MessageEncoder.Extensions;
// ReSharper disable InconsistentNaming

namespace Sinch.MessageEncoder.Messages;

public struct MessageHeaderTransport
{
    public readonly long HEADERS_LENGTH = default;
    public readonly long MSG_FROM = default;
    public readonly long MSG_TIMESTAMP = default;
    public readonly long MSG_TO = default;
    public readonly byte MSG_TYPE = default;
    private int HEADERS_COUNT = default;
    public byte[] HEADER_BYTES = Array.Empty<byte>();

    public MessageHeaderTransport()
    {
        MSG_FROM = 0;
        MSG_TIMESTAMP = 0;
        MSG_TO = 0;
        MSG_TYPE = 0;
        HEADERS_LENGTH = 0;
    }

    private MessageHeaderTransport
    (
        long msgFrom,
        long msgTo,
        long msgTimestamp,
        byte msgType,
        long headersLength,
        Span<byte> headerBytes
    ) : this()
    {
        MSG_FROM = msgFrom;
        MSG_TO = msgTo;
        MSG_TIMESTAMP = msgTimestamp;
        MSG_TYPE = msgType;
        HEADERS_LENGTH = headersLength;
        HEADER_BYTES = headerBytes.ToArray();
    }

    public static MessageHeaderTransport FromSpan(Span<byte> messageSpan, long headersLength)
    {
        return new(
            msgFrom: messageSpan.GetMessageFrom(),
            msgTo: messageSpan.GetMessageTo(),
            msgTimestamp: messageSpan.GetMessageTimestamp(),
            msgType: messageSpan.GetMessageType(),
            headersLength: headersLength,
            headerBytes: messageSpan.GetAllHeaders(headersLength)
        );
    }

    internal void AddHeader(byte[] byteArray)
    {
        HEADER_BYTES = HEADER_BYTES.Concat(byteArray).ToArray();
        HEADERS_COUNT++;
    }
}