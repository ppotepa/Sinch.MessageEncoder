using Sinch.MessageEncoder.Factories.Messages;
using Sinch.MessageEncoder.Factories.Serialization;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using Sinch.MessageEncoder.PoC.Diagnostics;
using System;
using System.Linq;

namespace Sinch.MessageEncoder.PoC
{
    internal class Program
    {
        //private static Span<byte> ExtractFromFile(string filePath)
        //{
        //    using MemoryStream inMemoryCopy = new();

        //    using (FileStream fileStream = File.OpenRead(filePath))
        //    {
        //        using DeflateStream decompressor = new(fileStream, CompressionMode.Decompress);
        //        decompressor.CopyTo(inMemoryCopy);
        //        inMemoryCopy.Position = 0;
        //    }

        //    Span<byte> span = new(new byte[inMemoryCopy.Length]);
        //    _ = inMemoryCopy.Read(span);
        //    inMemoryCopy.Dispose();
        //    return span;
        //}

        private static string LONG_STRING = string.Join("", Enumerable.Range(1, 16_000_000).Select(x => "A"));

        static void Main(string[] args)
        {
            //Span<byte> fromCompressedFileSpan = ExtractFromFile("testbinaries\\deflate.zlib");
            var delegateStopWatch = new DelegateStopwatch(Measure_2, Console.WriteLine);
            var executions = 500;
            var average = delegateStopWatch.Execute(executions);
            Console.WriteLine($"Average {average} ms. for {executions}");
        }

        private static readonly Action Measure_1 = () =>
        {
            byte[] binaryObject = GetBinaryObject();
            Message message = MessageFactory.Create(binaryObject);
            byte[] serialized = MessageFactory.Serialize(message);
            bool equal = serialized.SequenceEqual(binaryObject);
        };

        private static readonly Action Measure_2 = () =>
        {
            var message = new DefaultTextMessage
            {
                Headers = new DefaultTextMessageHeaders
                {
                    From = 1,
                    To = 2,
                    Timestamp = 12312312,
                    MessageType = 1,
                    AdditionalHeaders = null
                },
                Payload = new DefaultTextMessagePayload
                {
                    TextMessageBodyExtended = LONG_STRING,
                    Replies = 32,
                    TextMessageBody = "John"
                }
            };

            var payloadSerializer = SerializersFactory.CreatePayloadSerializer(typeof(DefaultTextMessagePayload));
            var headersSerializer = SerializersFactory.CreateHeadersSerializer(typeof(DefaultTextMessageHeaders));

            var result1 = payloadSerializer.Serialize(message.Payload);
            var result2 = headersSerializer.Serialize(message.Headers);

            //var result_3 =  new[] { result1, result2 }.SelectMany(bytes => bytes).ToArray();
        };


        private static byte[] GetBinaryObject()
        {
            return new BinaryMessageBuilder
            (
                from: 1,
                to: 2,
                timestamp: 1685193094,
                msgType: 1,
                headersLength: new long[] { 2 + 1, 2 + 1, 2 + 1, 3 * 16 }.Sum()
            )
            .AddHeader("test-header", (byte)254)
            .AddHeader("test-header-2", (byte)100)
            .AddHeader("test-header-3", (byte)50)
            .AddHeader("test-header-5", "AAAAAAAAAAAAAAAA")
            .AddHeader("test-header-6", "BBBBBBBBBBBBBBBB")
            .AddHeader("test-header-7", "OKTQYKCIHBOLROJI")
            .AddPayload(new DefaultTextMessagePayload { TextMessageBody = "John", TextMessageBodyExtended = LONG_STRING, Replies = 32})
            .Serialize();
        }
    }
}

