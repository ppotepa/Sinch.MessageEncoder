namespace Sinch.MessageEncoder.Messages
{
    public abstract class Message
    {
        protected Message(object headers, object payload)
        {
            this._headers = headers;
            this._payload = payload;
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
                if (_headers is null) _headers = new THeadersType();
                return _headers as THeadersType;
            }
        }

        public abstract int HeadersCount { get; }
        public override TPayloadType Payload
        {
            get
            {
                if (_payload is null) _payload = new TPayloadType();
                return _payload as TPayloadType;
            }
        }

        protected Message(object headers, object payload) : base(headers, payload)
        {
        }
    }
}
