using Sinch.MessageEncoder.Messages;
using System;
using System.IO;
using System.IO.Compression;
using Sinch.MessageEncoder.Messages.Default;

namespace Sinch.MessageEncoder.PoC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            long iter = 0;

            using MemoryStream inMemoryCopy = new MemoryStream();
            using (FileStream fileStream = File.OpenRead("testbinaries\\deflate.zlib"))
            {
                using DeflateStream decompressor = new DeflateStream(fileStream, CompressionMode.Decompress);
                decompressor.CopyTo(inMemoryCopy);
                inMemoryCopy.Position = 0;
            }

            var span = new Span<byte>(new byte[inMemoryCopy.Length]);
            _ = inMemoryCopy.Read(span);

            while (iter++ != -1)
            {
                var messageTransport = MessageTransport.FromSpan(span);
                var message = MessageFactory(messageTransport);

                var a = message.Payload;
                var payload = ((Message)message).Payload;

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

