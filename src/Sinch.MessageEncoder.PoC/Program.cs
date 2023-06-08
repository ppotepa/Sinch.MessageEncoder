using Sinch.MessageEncoder.Factories.Messages;
using Sinch.MessageEncoder.Factories.Serialization;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using Sinch.MessageEncoder.PoC.Builders;
using Sinch.MessageEncoder.PoC.Diagnostics;
using Sinch.MessageEncoder.Serializers;
using System;
using System.Linq;
using Sinch.MessageEncoder.Extensions;

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

        private static readonly Action Measure_1 = () =>
        {
            ReadOnlySpan<byte> binaryObject = GetBinaryObject();

            Message message = MessageFactory.Create(binaryObject);
            ReadOnlySpan<byte> serialized = MessageFactory.Serialize(message);

            //var fromBinary = Message.FromBytes(serialized);
            bool equal = serialized.SequenceEqual(binaryObject);

            for (var i = 0; i < serialized.Length; i++)
            {
                if (serialized[i] != binaryObject[i])
                {
                    Console.WriteLine(new { index = i });
                    Console.WriteLine($"s:{serialized[i]}, b:{binaryObject[i]}");
                }
            }
        };

        private static readonly Action Measure_2 = () =>
        {
            var message = new DefaultTextMessage
            {
                Headers = new()
                {
                    From = 1,
                    To = 2,
                    Timestamp = 12312312,
                    MessageType = 1
                },

                Payload = new()
                {
                    TextMessageBody = "John"
                }
            };

            var payloadSerializer = SerializersFactory.CreateSerializer<IPayloadSerializer>(typeof(DefaultTextMessagePayload));
            var headersSerializer = SerializersFactory.CreateSerializer<IHeadersSerializer>(typeof(DefaultTextMessageHeaders));

            var result1 = payloadSerializer.Serialize(message.Payload);
            var result2 = headersSerializer.Serialize(message.Headers);
        };

        private static string LONG_STRING => string.Join("", Enumerable.Range(1, new Random().Next(1, 1024 * 1024 * 16)).Select(x => "A"));

        private static ReadOnlySpan<byte> GetBinaryObject()
        {
            return MessageBuilder<DefaultTextMessageHeaders, DefaultTextMessagePayload>.CreateBuilder()
                .From(1)
                .To(2)
                .Timestamp(DateTime.Now.Ticks)
                .MsgType(1)
                .AddHeader("recipient-name", "Joe")
                .AddHeader("sender-name", "Anne")
                .AddHeader("is-message-unread", true)
                .EndHeaders()
                .AddPayloadProperty(nameof(DefaultTextMessagePayload.TextMessageBody), "Test Message body")
                .GetBinary();
        }

        static void Main(string[] args)
        {
            var builder = MessageBuilder<DefaultTextMessageHeaders, DefaultTextMessagePayload>.CreateBuilder();

            var message = builder
                .From(1)
                .To(2)
                .Timestamp(DateTime.Now.Ticks)
                .MsgType(1)
                .AddHeader("recipient-name", "Joe")
                .AddHeader("sender-name", "Anne")
                .AddHeader("is-message-unread", true)
                .EndHeaders()
                .AddPayloadProperty(nameof(DefaultTextMessagePayload.TextMessageBody), "Test Message body")
                .Build();

            //Span<byte> fromCompressedFileSpan = ExtractFromFile("testbinaries\\deflate.zlib");
            var delegateStopWatch = new DelegateStopwatch(Measure_1, Console.WriteLine);
            var executions = -1;
            var average = delegateStopWatch.Execute(executions);
            Console.WriteLine($"Average {average} ms. for {executions}");
        }
    }
}

