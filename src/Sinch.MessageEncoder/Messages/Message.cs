namespace Sinch.MessageEncoder.Messages
{
    public abstract class Message
    {
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
                if (base.Headers is not null) return (THeadersType)base.Headers;
                base.Headers = new THeadersType();
                return (THeadersType)base.Headers;
            }
        }

        public abstract int HeadersCount { get; }
        public override TPayloadType Payload
        {
            get
            {
                if (base.Payload is not null) return (TPayloadType)base.Payload;
                Payload = new TPayloadType();
                return Payload;
            }
        }
    }
}
