using System;
using System.Collections.Generic;
using Sinch.MessageEncoder.Extensions;

namespace Sinch.MessageEncoder.Messages;

public struct MessageHeaderTransport
{
    public readonly long MSG_FROM = default;
    public readonly long MSG_TIMESTAMP = default;
    public readonly long MSG_TO = default;
    public readonly byte MSG_TYPE = default;
    public readonly long HEADERS_LENGTH = default;
    private int HEADERS_COUNT = default;

    internal Dictionary<int, byte[]> AdditionalHeaders = new();

    public MessageHeaderTransport()
    {
        MSG_FROM = 0;
        MSG_TIMESTAMP = 0;
        MSG_TO = 0;
        MSG_TYPE = 0;
        HEADERS_LENGTH = 0;
    }

    private MessageHeaderTransport(long msgFrom, long msgTo, long msgTimestamp, byte msgType, long headersLength) : this()
    {
        MSG_FROM = msgFrom;
        MSG_TO = msgTo;
        MSG_TIMESTAMP = msgTimestamp;
        MSG_TYPE = msgType;
        HEADERS_LENGTH = headersLength;
    }

    internal void AddHeader(byte[] byteArray)
    {
        AdditionalHeaders[++HEADERS_COUNT] = byteArray;
    }


    public static MessageHeaderTransport FromSpan(Span<byte> messageSpan, long headersLength)
    {
        return new(
            msgFrom: messageSpan.GetMessageFrom(), 
            msgTo: messageSpan.GetMessageTo(),
            msgTimestamp: messageSpan.GetMessageTimestamp(), 
            msgType: messageSpan.GetMessageType(),
            headersLength: headersLength
        );
    }
}