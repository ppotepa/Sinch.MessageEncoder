using Sinch.MessageEncoder.PoC.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.PoC
{
    //Helper Class that was used to create a Binary-Format array that represents Binary-Message-Transport
    internal class BinaryMessageBuilder
    {
        private object _payloadObject;
        private List<KeyValuePair<string, object>> Headers = new();
        public BinaryMessageBuilder(long from, long to, long timestamp, byte msgType)
        {
            this.From = from;
            this.To = to;
            this.Timestamp = timestamp;
            this.MsgType = msgType;
        }

        public long From { get; set; }
        public byte MsgType { get; set; }
        public long Timestamp { get; set; }
        public long To { get; set; }


        public BinaryMessageBuilder AddHeader<THeaderType>(string name, THeaderType data)
        {
            this.Headers.Add(new KeyValuePair<string, object>(name, data));
            return this;
        }

        public BinaryMessageBuilder AddPayload<TObject>(TObject @object)
        {
            this._payloadObject = @object;
            return this;
        }

        public byte[] Serialize()
        {
            var result = new []
            {
                From.ToByteArray(),
                To.ToByteArray(),
                Timestamp.ToByteArray(),
                new []{ MsgType },
                Headers.Select(kvp =>
                {
                    byte[] byteArr = kvp.Value.ToByteArray();
                    byte[] result = ((short)byteArr.Length).ToByteArray().Concat(byteArr).ToArray();
                    return result;
                })
                .SelectMany(bytes => bytes).ToArray(),
            };

            var flatten = result.SelectMany(@byte => @byte).ToArray();
            return flatten;
        }
    }
}

