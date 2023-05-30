using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using System;
using System.IO;
using System.IO.Compression;

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
            return span;
        }

        static void Main(string[] args)
        {
            var dateFromTicks = DateTime.FromBinary(1685193094);
            long iter = 0;

            var res = 24 + (2 + 1) + (2 + 1) + (2 + 1) + 2 + 3;

            byte[] binaryObject = new BinaryMessageBuilder(1, 2, 1685193094, 1, 24 + (2 + 1) + (2 + 1) + (2 + 1) + 2 + 3)
                .AddHeader("test-header", (byte)1)
                .AddHeader("test-header2", (byte)100)
                .AddHeader("test-header3", (byte)100)
                .AddHeader("test-header5", "123")
                .AddPayload(new DefaultTextMessagePayload{Name = "John", Surname = "Doe", Text = "This is my text."})
                .Serialize();

            //.AddPayload(new { Filename = "movie.mp4", Length = 12, Payload = new byte[]{ 01, 10, 225, 25} })
            //.Serialize();

            Span<byte> fromCompressedFileSpan = ExtractFromFile("testbinaries\\deflate.zlib");

            while (iter++ != -1)
            {
                MessageTransport messageTransport = MessageTransport.FromSpan(binaryObject);
                Message message = MessageFactory(messageTransport);

                object a = message.Payload;
                object payload = message.Payload;

                if (iter % 100000000 is 0)
                {
                    Console.WriteLine(iter);
                }
            }
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

