using System;
using System.Collections.Generic;
using System.IO;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class Message
    {
        private readonly MessageHeaderTransport _headerHeaderTransport;
        private readonly byte PayloadStream;

        protected Message(MessageHeaderTransport headerTransport, Span<byte> payloadSpan)
        {
            this._headerHeaderTransport = headerTransport;
        }
        
        public object Payload { get; set; }
        public byte[] PayloadArray { get; set; }
    }

    public abstract class Message<TPayloadType> : Message where TPayloadType : Payload
    {
        public abstract int HeadersCount { get; }

        protected Message(MessageHeaderTransport headerTransport, Span<byte> payloadSpan) : base(headerTransport, payloadSpan)
        {
        }

        public MessageHeader Header { get; init; }
        public MessageHeaderTransport HeaderTransport { get; set; }

        public new TPayloadType Payload
        {
            get => (TPayloadType)base.Payload;
            set => base.Payload = value;
        }

        public MemoryStream PayloadStream { get; set; }
    }
}
