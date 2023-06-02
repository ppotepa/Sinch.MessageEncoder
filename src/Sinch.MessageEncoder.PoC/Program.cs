using Sinch.MessageEncoder.Factories.Messages;
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
            byte[] binaryObject = GetBinaryObject();

            Message message = MessageFactory.Create(binaryObject);
            byte[] serialized = MessageFactory.Serialize(message);

            Message msg2 = MessageFactory.Create(serialized);
            byte[] serialized2 = MessageFactory.Serialize(msg2);

            var equals = serialized.SequenceEqual(serialized2);
        }

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
                .AddPayload(new DefaultTextMessagePayload { TextMessageBody = "John" })
                .Serialize();
        }
    }
}

