using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using Sinch.MessageEncoder.PoC.Diagnostics;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Sinch.MessageEncoder.PoC
{
    internal class Program
    {
        private static Span<byte> ExtractFromFile(string filePath)
        {
            using MemoryStream inMemoryCopy = new();

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                using DeflateStream decompressor = new(fileStream, CompressionMode.Decompress);
                decompressor.CopyTo(inMemoryCopy);
                inMemoryCopy.Position = 0;
            }

            Span<byte> span = new(new byte[inMemoryCopy.Length]);
            _ = inMemoryCopy.Read(span);
            inMemoryCopy.Dispose();
            return span;
        }

        static void Main(string[] args)
        {
            //Span<byte> fromCompressedFileSpan = ExtractFromFile("testbinaries\\deflate.zlib");
            var delegateStopWatch = new DelegateStopwatch(Measure, Console.WriteLine);
            var executions = 100;
            var average = delegateStopWatch.Execute(executions);
            Console.WriteLine($"Average {average} ms. for {executions}");
        }

        private static void Measure()
        {
            byte[] binaryObject = new BinaryMessageBuilder
            (
                from: 1,
                to: 2,
                timestamp: 1685193094,
                msgType: 1,
                headersLength: new long[] { 24, 2 + 1, 2 + 1, 2 + 1, 2, 3 }.Sum()
            )
            .AddHeader("test-header", (byte)  254)
            .AddHeader("test-header2", (byte) 100)
            .AddHeader("test-header3", (byte) 50)
            .AddHeader("test-header5", "AAAAAAAAAAAAAAAA")
            .AddPayload(new DefaultTextMessagePayload { SerializedText = "John" })
            .Serialize();

            MessageTransport messageTransport = MessageTransport.FromSpan(binaryObject);
            Message message = MessageFactory(messageTransport);

            object a = message.Payload;
            object payload = message.Payload;
        }

        static Message MessageFactory(MessageTransport messageTransport)
        {
            return messageTransport.HeaderTransportInfo.MSG_TYPE switch
            {
                1 => new DefaultTextMessage(messageTransport.HeaderTransportInfo, messageTransport.BinaryPayload),
                _ => default
            };
        }
    }
}

