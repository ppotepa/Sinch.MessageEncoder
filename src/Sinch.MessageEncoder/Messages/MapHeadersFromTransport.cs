using System;
using Sinch.MessageEncoder.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sinch.MessageEncoder.Messages
{
    public abstract class MessageHeader
    {
        protected MessageHeader()
        {
        }

        public Dictionary<string, string> AdditionalHeaders { get; set; }

        public virtual byte[] DefaultBytes => new object[] {
            From, To, Timestamp, MessageType, HeadersLength, AdditionalHeaderBytes
        }
        .SelectMany(@object => @object.ToByteArray())
        .ToArray();

        public long From { get; internal set; }
        public long HeadersLength { get; internal set; }
        public byte MessageType { get; internal set; }
        public long Timestamp { get; internal set; }
        public long To { get; internal set; }
        internal byte[] AdditionalHeaderBytes { get; set; }

        internal string MyTestObject { get; set; }

        public THeader Map<THeader, TProperty>(TProperty value, Expression<Func<THeader, TProperty>> property)
            where THeader : MessageHeader
        {
            var body = Expression.MakeBinary(
                ExpressionType.Assign, property.Body, Expression.Constant(value)
            );

            var action = Expression.Lambda<Action<THeader>>(body, property.Parameters).Compile();
            action((THeader)this);
            return (THeader)this;
        }

        public virtual void MapFromTransport(MessageHeaderTransport headersTransport)
        {
            this.AdditionalHeaders = new Dictionary<string, string>();
        }
    }
}