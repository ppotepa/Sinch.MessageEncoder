using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Sinch.MessageEncoder.Messages;

namespace Sinch.MessageEncoder.PoC
{
    internal class Program
    {
        //1024x1024
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum MSG_HEADER_TYPE
        {
            CONTENT_HEADER_TYPE = 1,
            MSG_SIZE = 2,
            TIMESTAMP = 3,
            CONTAINS_IMAGES = 4
        }

        static unsafe void Main(string[] args)
        {
            var testMessage = new Message
            {
                Header = new MessageHeader
                {
                    [nameof(MSG_HEADER_TYPE.CONTENT_HEADER_TYPE)] = "STRING",
                    [nameof(MSG_HEADER_TYPE.MSG_SIZE)] = "1024",
                    [nameof(MSG_HEADER_TYPE.TIMESTAMP)] = DateTime.Now.Ticks.ToString(),
                    [nameof(MSG_HEADER_TYPE.CONTAINS_IMAGES)] = "false",
                    ["1"] = "false",
                },
                Payload = System.Text.Encoding.ASCII.GetBytes(Enumerable.Range(1, 1024).Select(integer => (char)integer).ToArray())
            };

            byte[] bytes = new byte[sizeof(Message)];
           

            // serialize
            fixed (byte* p = bytes)
            {
                var typed = (Message*)p;
                *typed = testMessage;
            }

            Console.WriteLine(BitConverter.ToString(bytes));
            
        }
    }
}
