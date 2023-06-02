using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Factories.Serialization;
using Sinch.MessageEncoder.Messages;
using System.Collections.Generic;
using System.Linq;

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
            
            byte[] fromByteArray = From.ToByteArray();
            byte[] toByteArray = To.ToByteArray();
            byte[] timestampByteArray = Timestamp.ToByteArray();
            byte[] msgTypeByteArray = { MsgType };
            byte[] headersByteArray = Headers.Select(kvp =>
            {
                byte[] byteArr = kvp.Value.ToByteArray();
                byte[] result = ((short)byteArr.Length).ToByteArray().Concat(byteArr).ToArray();
                return result;
            })
            .SelectMany(bytes => bytes).ToArray();

            // ReSharper disable once RedundantCast - in this case it is not Redunant,
            // because it makes our byteArray 8-byte long instead of 4-byte
            byte[] headersByteArrayLength = ((long)(headersByteArray.Length)).ToByteArray();
            byte[] payloadByteArray = PayloadSerializerFactory.CreateSerializer(Payload?.GetType()).Serialize(Payload);

            var result = new object[]
            {
                fromByteArray,
                toByteArray,
                timestampByteArray,
                msgTypeByteArray,
                headersByteArrayLength,
                headersByteArray,
                payloadByteArray
            };

            #region Refactored
            // var result = new object[]
            //{
            //     From,
            //     To,
            //     Timestamp,
            //     MsgType,
            //     Headers.Count,
            //     Headers.Select(h => h.Value),
            //     Payload
            //};
            #endregion

            byte[] flatten = result.SelectMany(@object => @object.ToByteArray()).ToArray();
            return flatten;
        }

        //public Message Deserialize<TMessage>(byte[] byteArray)
        //{
        //    byte[][] result = new[]
        //    {
        //        From.ToByteArray(),
        //        To.ToByteArray(),
        //        Timestamp.ToByteArray(),
        //        new []{ MsgType },
        //        Headers.Select(kvp =>
        //            {
        //                byte[] byteArr = kvp.Value.ToByteArray();
        //                byte[] result = ((short)byteArr.Length).ToByteArray().Concat(byteArr).ToArray();
        //                return result;
        //            })
        //            .SelectMany(bytes => bytes).ToArray(),
        //    };

        //    byte[] flatten = result.SelectMany(@byte => @byte).ToArray();
        //    return flatten;
        //}
    }
}

