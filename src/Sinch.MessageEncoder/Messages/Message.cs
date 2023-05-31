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
        public object Headers { get; set; }
    }

    public abstract class Message<THeadersType, TPayloadType> : Message 
        where TPayloadType : Payload, new()
        where THeadersType : MessageHeader, new()
    {
        private THeadersType _headers;
        private TPayloadType _payload;

        protected Message(MessageHeaderTransport headerTransport, Span<byte> payloadSpan) : base(headerTransport, payloadSpan)
        {
        }

        public MessageHeader Header { get; init; }
        public new THeadersType Headers
        {
            get
            {
                if (_headers is not null) return _headers;
                _headers = new THeadersType();
                return _headers;
            }
            set => base.Payload = value;
        }

        public abstract int HeadersCount { get; }
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
