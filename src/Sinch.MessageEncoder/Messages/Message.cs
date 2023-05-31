using System;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class Message
    {
        private readonly MessageHeaderTransport _headerTransport;
        protected Message(MessageHeaderTransport headerTransport, Span<byte> payloadSpan)
        {
            this._headerTransport = headerTransport;
        }

        public object Payload { get; set; }
    }

    public abstract class Message<TPayloadType> : Message where TPayloadType : Payload, new()
    {
        private TPayloadType _payload;
        public abstract int HeadersCount { get; }
        protected Message(MessageHeaderTransport headerTransport, Span<byte> payloadSpan) : base(headerTransport, payloadSpan)
        {

        }

        public MessageHeader Header { get; init; }
        public MessageHeaderTransport HeaderTransport { get; protected set; }

        public new TPayloadType Payload
        {
            get
            {
                if (_payload is not null) return _payload;
                _payload = new TPayloadType();
                return _payload;
            }
            set => base.Payload = value;
        }
    }
}
