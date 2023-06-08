using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class MessageHeader : IBuildable
    {
        internal readonly Dictionary<string, object> Headers = new Dictionary<string, object>();

        protected MessageHeader(params object[] headers)
        {
        }

        public long From { get; set; }

        public byte MessageType { get; set; }

        public long Timestamp { get; set; }

        public long To { get; set; }

        public long HeadersLength 
            => this.Headers.Values.Select(x => x.ToByteArray().Length).Sum() + (this.Headers.Count * 2);

        internal virtual byte[] DefaultBytes => new object[] {
            From, To, Timestamp, MessageType
        }
        .SelectMany(@object => @object.ToByteArray())
        .ToArray();

        public object this[string index]
        {
            get => Headers[index];
            set
            {
                if (!Headers.ContainsKey(index))
                {
                    Headers[index] = value;
                }
            }
        }

        public virtual void Apply(MessageHeaderTransport headersTransport)
        {
            this.From = headersTransport.MSG_FROM;
            this.To = headersTransport.MSG_TO;
            this.MessageType = headersTransport.MSG_TYPE;
            this.Timestamp = headersTransport.MSG_TIMESTAMP;
            this.MessageType = headersTransport.MSG_TYPE;
        }

        public THeader Map<THeader, TProperty>(TProperty value, Expression<Func<THeader, TProperty>> property, Func<byte[], byte[]> span)
            where THeader : MessageHeader
        {
            var body = Expression.MakeBinary(
                ExpressionType.Assign, property.Body, Expression.Constant(value)
            );

            var action = Expression.Lambda<Action<THeader>>(body, property.Parameters).Compile();
            action((THeader)this);
            return (THeader)this;
        }
    }

    public interface IBuildable
    {
    }
}