using System;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class Message
    {
        protected Message(object headers, object payload)
        {
            this._headers = headers ?? throw new ArgumentNullException(nameof(headers));
            this._payload = payload ?? throw new ArgumentNullException(nameof(payload));
        }

        protected Message()
        {
        }

        protected object _headers;
        protected object _payload;

        public virtual object Headers
        {
            get => _headers;
            set => _headers = value;
        }

        public virtual object Payload
        {
            get => _payload;
            set => _payload = value;
        }
    }

    public abstract class Message<THeadersType, TPayloadType> : Message
        where TPayloadType : Payload, new()
        where THeadersType : MessageHeader, new()
    {
        public override THeadersType Headers
        {
            get
            {
                _headers ??= new THeadersType();
                return _headers as THeadersType;
            }
        }

        public abstract int HeadersCount { get; }
        public override TPayloadType Payload
        {
            get
            {
                _payload ??= new TPayloadType();
                return _payload as TPayloadType;
            }
        }

        protected Message(object headers, object payload) : base(headers, payload)
        {
        }
    }
}
