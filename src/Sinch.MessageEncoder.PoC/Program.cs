using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;

namespace Sinch.MessageEncoder.PoC
{
    internal class Program
    {
        private static Span<byte> ExtractFromFile(string filePath)
        {
            using MemoryStream inMemoryCopy = new MemoryStream();
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                using DeflateStream decompressor = new DeflateStream(fileStream, CompressionMode.Decompress);
                decompressor.CopyTo(inMemoryCopy);
                inMemoryCopy.Position = 0;
            }

            var span = new Span<byte>(new byte[inMemoryCopy.Length]);
            _ = inMemoryCopy.Read(span);
            return span;
        }

        static void Main(string[] args)
        {
            long iter = 0;

            byte[] binaryObject = new BinaryMessageBuilder(1, 2, 1685193094, 1)
                .AddHeader("test-header", (byte)1)
                .AddHeader("test-header2", (byte)100)
                .AddHeader("test-header3", (byte)100)
                .AddHeader("test-header5", "123")
                .Serialize();

            //.AddPayload(new { Filename = "movie.mp4", Length = 12, Payload = new byte[]{ 01, 10, 225, 25} })
            //.Serialize();

            Span<byte> fromCompressedFileSpan = ExtractFromFile("testbinaries\\deflate.zlib");

            while (iter++ != -1)
            {
                var messageTransport = MessageTransport.FromSpan(binaryObject);
                var message = MessageFactory(messageTransport);

                var a = message.Payload;
                var payload = message.Payload;

                if (iter % 100000000 is 0)
                {
                    Console.WriteLine(iter);
                }
            }
        }

        static Message MessageFactory(MessageTransport messageTransport)
        {
            switch (messageTransport.HeaderTransportInfo.MSG_TYPE)
            {
                case 1: return new DefaultTextMessage(messageTransport.HeaderTransportInfo, messageTransport.BinaryPayload);
            }

            return default;
        }
    }
}

