using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class MessageHeader
    {
        private readonly Dictionary<string, object> _headers = new Dictionary<string, object>();

        protected MessageHeader()
        {
        }

        public virtual byte[] DefaultBytes => new object[] {
            From, To, Timestamp, MessageType, HeadersLength,
        }
        .SelectMany(@object => @object.ToByteArray())
        .ToArray();

        public long From { get; set; }

        public long HeadersLength { get; set; }

        public byte MessageType { get; set; }

        public long Timestamp { get; set; }

        public long To { get; set; }

        public object this[string index]
        {
            get => _headers[index];
            set => _headers[index] = value;
        }
        public virtual void Apply(MessageHeaderTransport headersTransport)
        {
            this.From = headersTransport.MSG_FROM;
            this.To = headersTransport.MSG_TO;
            this.MessageType = headersTransport.MSG_TYPE;
            this.Timestamp = headersTransport.MSG_TIMESTAMP;
            this.MessageType = headersTransport.MSG_TYPE;
            this.HeadersLength = headersTransport.HEADERS_LENGTH;
        }

        public THeader Map<THeader, TProperty>(TProperty value, Expression<Func<THeader, TProperty>> property,
                    Func<byte[], byte[]> span)
            where THeader : MessageHeader
        {
            var bytes = typeof(TProperty).Name;
            var body = Expression.MakeBinary(
                ExpressionType.Assign, property.Body, Expression.Constant(value)
            );

            var action = Expression.Lambda<Action<THeader>>(body, property.Parameters).Compile();
            action((THeader)this);
            return (THeader)this;
        }
    }
}