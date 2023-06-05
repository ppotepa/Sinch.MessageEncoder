using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using System.Collections.Generic;
using System.Linq;
using Sinch.MessageEncoder.Factories.Serialization;

namespace Sinch.MessageEncoder.PoC
{
    //Helper Class that was used to create a Binary-Format array that represents Binary-Message-Transport
    internal class BinaryMessageBuilder
    {
        private readonly List<KeyValuePair<string, object>> Headers = new();
        private Payload Payload;

        public BinaryMessageBuilder(long from, long to, long timestamp, byte msgType, long headersLength)
        {
            this.From = from;
            this.To = to;
            this.Timestamp = timestamp;
            this.MsgType = msgType;
            this.HeadersLength = headersLength;
        }

        public long From { get; set; }
        public long HeadersLength { get; set; }
        public byte MsgType { get; set; }
        public long Timestamp { get; set; }
        public long To { get; set; }


        public BinaryMessageBuilder AddHeader<THeaderType>(string name, THeaderType data)
        {
            this.Headers.Add(new KeyValuePair<string, object>(name, data));
            return this;
        }

        public BinaryMessageBuilder AddPayload<TObject>(TObject @object) where TObject : Payload
        {
            this.Payload = @object;
            return this;
        }

        public byte[] Serialize()
        {
            Payload ??= Payload.Empty;

            ArrangeBytes
            (
                out var fromByteArray,
                out var toByteArray,
                out var timestampByteArray,
                out var msgTypeByteArray,
                out var headersByteArray,
                out var headersByteArrayLength,
                out var payloadByteArray
            );

            return new[]
            {
                fromByteArray,
                toByteArray,
                timestampByteArray,
                msgTypeByteArray,
                headersByteArrayLength,
                headersByteArray,
                payloadByteArray
            }
            .SelectMany(@object => @object).ToArray();
        }

        private void ArrangeBytes
        (
            out byte[] fromByteArray, out byte[] toByteArray, out byte[] timestampByteArray, out byte[] msgTypeByteArray,
            out byte[] headersByteArray, out byte[] headersByteArrayLength, out byte[] payloadByteArray)
        {
            fromByteArray = From.ToByteArray();
            toByteArray = To.ToByteArray();
            timestampByteArray = Timestamp.ToByteArray();
            msgTypeByteArray = new[] { MsgType };

            headersByteArray = Headers.Select(pair =>
            {
                byte[] byteArr = pair.Value.ToByteArray();
                byte[] result = ((short)byteArr.Length).ToByteArray().Concat(byteArr).ToArray();
                return result;
            })
            .SelectMany(bytes => bytes)
            .ToArray();

            // ReSharper disable once RedundantCast - in this case it is not Redunant,
            // because it makes our byteArray 8-byte long instead of 4-byte
            headersByteArrayLength = ((long)headersByteArray.Length).ToByteArray();
            payloadByteArray = SerializersFactory.CreatePayloadSerializer(Payload?.GetType()).Serialize(Payload);
        }
    }
}

