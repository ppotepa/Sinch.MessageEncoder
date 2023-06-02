using System;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.Serialization
{
    public interface IHeadersSerializer
    {
        public THeaders Deserialize<THeaders>(Span<byte> payload) where THeaders : MessageHeader, new();
        public byte[] Serialize<THeaders>(THeaders payload) where THeaders : MessageHeader;
    }
}