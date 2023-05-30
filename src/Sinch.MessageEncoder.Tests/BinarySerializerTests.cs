using NUnit.Framework;
using Sinch.MessageEncoder.PoC;
using Sinch.MessageEncoder.PoC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Tests
{
    public class Tests
    {
        [Test]
        public void Binary_Serializer_Serializes_Basic_Headers_Correctly()
        {
            byte[] assertBytes =
            {
                1,   0, 0,   0,   0, 0, 0, 0,
                2,   0, 0,   0,   0, 0, 0, 0,
                134, 1, 114, 100, 0, 0, 0, 0,
                1,
                //2, 100, 0,
                //2, 16, 39,
                //4, 0, 202, 154, 59,
                //8, 0, 16, 165, 212, 232, 0, 0, 0
            };

            byte[] binaryMessageBuilder = new BinaryMessageBuilder((long)1, (long)2, (long)1685193094, (byte)1)
                //.AddHeader("byte", (byte)100)
                //.AddHeader("short", (short)10_000)
                //.AddHeader("int", (int)1_000_000_000)
                //.AddHeader("long", (long)1_000_000_000_000)
                .Serialize();

            Assert.IsTrue(assertBytes.SequenceEqual(binaryMessageBuilder)); ;
        }

        [Test]
        public void Binary_Serializer_Serializes_Basic_Headers_Correctly_With_Additional_Headers()
        {
            byte[] assertBytes =
            {
                     1, 0, 0, 0, 0, 0, 0, 0,                //  FROM                [8] bytes
                     2, 0, 0, 0, 0, 0, 0, 0,                //  TO                  [8] bytes
                     134, 1, 114, 100, 0, 0, 0, 0,          //  TIMESTAMP           [8] bytes
                1,                                          //  MSG_TYPE            [1] byte 
                1, 0, 100,                                  //  HEADER_1            [2 bytes, 1 byte]
                2, 0, 100, 0,                               //  HEADER_2            [2 bytes, 2 bytes]
                4, 0, 100, 0, 0, 0,                         //  HEADER_3            [2 bytes, 4 bytes]
                8, 0, 100, 0, 0, 0, 0, 0, 0, 0,             //  HEADER_4            [2 bytes, 8 bytes]
                32, 0,  97, 97, 97, 97, 97, 97, 97, 97,     //  HEADER_4            [2 bytes, 32 bytes (string)]
                        97, 97, 97, 97, 97, 97, 97, 97,      
                        97, 97, 97, 97, 97, 97, 97, 97,
                        97, 97, 97, 97, 97, 97, 97, 97       
            };

            var defaultSerializerInput = new object[]
            {
                (long)1,
                (long)2,
                (long)1685193094,
                (byte)1,
                (short)1, (byte)100,
                (short)2, (short)100,
                (short)4, (int)100,
                (short)8, (long)100,
                (short)32, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            };
            IEnumerable<byte> defaultSerializerResult = defaultSerializerInput.SelectMany(integer => integer.ToByteArray());

            IEnumerable<byte> binarySerializerResult = defaultSerializerInput.SelectMany(data =>
            {
                var result = data switch
                {
                    //out of some reason BitConverter interprets single byte as 2-byte digit ?? weird
                    byte @byte => BitConverter.GetBytes(@byte).Take(1),
                    short @short => BitConverter.GetBytes(@short).Take(2),
                    int @int => BitConverter.GetBytes(@int).Take(4),
                    long @long => BitConverter.GetBytes(@long).Take(8),
                    string @string => System.Text.Encoding.ASCII.GetBytes(@string),
                    _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
                };

                return result;
            });

            byte[] binaryMessageBuilder = new BinaryMessageBuilder((long)1, (long)2, (long)1685193094, (byte)1)
                .AddHeader("byte", (byte)100)
                .AddHeader("short", (short)100)
                .AddHeader("int", (int)100)
                .AddHeader("long", (long)100)
                .AddHeader("string", string.Join(string.Empty, Enumerable.Range(0, 32).Select(i => "a")))
                .Serialize();

            bool[] conditions = {
                assertBytes.SequenceEqual(binaryMessageBuilder),        //check against our BinaryBuilder
                assertBytes.SequenceEqual(defaultSerializerResult),     //check against our ToByteArray() extensions method
                assertBytes.SequenceEqual(binarySerializerResult)       //check against built-in BitConverter
            };

            Assert.IsTrue(conditions.All(condition => condition));
        }

        [Test]
        public void My_Stupid_BitConversion_Works()
        {
            Assert.IsTrue(BitConverter.GetBytes(1).SequenceEqual(1.ToByteArray()));
            Assert.IsTrue(BitConverter.GetBytes(1_000).SequenceEqual(1_000.ToByteArray()));
            Assert.IsTrue(BitConverter.GetBytes(1_000_000).SequenceEqual(1_000_000.ToByteArray()));
            Assert.IsTrue(BitConverter.GetBytes(1_000_000_000).SequenceEqual(1_000_000_000.ToByteArray()));
            Assert.IsTrue(BitConverter.GetBytes(1_000_000_000_000).SequenceEqual(1_000_000_000_000.ToByteArray()));
        }

        [SetUp]
        public void Setup()
        {
        }
    }
}