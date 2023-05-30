using System.Collections.Generic;

namespace Sinch.MessageEncoder.Messages;

public struct MessageHeaderTransport
{
    public long MSG_FROM = default;
    public long MSG_TIMESTAMP = default;
    public long MSG_TO = default;
    public byte MSG_TYPE = default;
    public long PAYLOAD_START_INDEX = default;

    internal Dictionary<int, byte[]> AdditionalHeaders = new();
    public MessageHeaderTransport()
    {
        MSG_FROM = 0;
        MSG_TIMESTAMP = 0;
        MSG_TO = 0;
        MSG_TYPE = 0;
        PAYLOAD_START_INDEX = 0;
    }

    internal void AddHeader(byte[] byteArray)
    {
        //AdditionalHeaders[++HEADERS_COUNT] = byteArray;
    }
}