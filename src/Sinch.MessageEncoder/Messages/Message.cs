using System;
using System.IO;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class Message
    {
        private readonly MessageHeaderTransport HeaderTransport;
        private readonly byte PayloadStream;

        protected Message(MessageHeaderTransport transport, Span<byte> payloadSpan)
        {
            this.HeaderTransport = transport;
        }
        
        public object Payload { get; set; }
        public byte[] PayloadArray { get; set; }
    }

    public abstract class Message<TPayloadType> : Message where TPayloadType : IPayload
    {
        public abstract int HeadersCount { get; }

        protected Message(MessageHeaderTransport transport, Span<byte> payloadSpan) : base(transport, payloadSpan)
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
