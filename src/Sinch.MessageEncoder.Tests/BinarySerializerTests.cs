using NUnit.Framework;
using Sinch.MessageEncoder.PoC;
using Sinch.MessageEncoder.PoC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace Sinch.MessageEncoder.Tests
{
    public class Tests
    {
        private const string STRING_32_BYTES = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        [Test]
        public void Binary_Serializer_Serializes_Basic_Headers_Correctly()
        {
            byte[] assertBytes =
            {
                1, 0, 0, 0, 0, 0, 0, 0,
                2, 0, 0, 0, 0, 0, 0, 0,
                134, 1, 114, 100, 0, 0, 0, 0,
                1
            };

            byte[] binaryMessageBuilder = new BinaryMessageBuilder(1, 2, 1685193094, 1)
                .Serialize();

            Assert.That(assertBytes.SequenceEqual(binaryMessageBuilder), Is.True); ;
        }

        [Test]
        public void Binary_Serializer_Serializes_Basic_Headers_Correctly_With_Additional_Headers()
        {
            byte[] assertBytes =
            {
                        1, 0, 0, 0, 0, 0, 0, 0,             //  FROM                [8] bytes
                        2, 0, 0, 0, 0, 0, 0, 0,             //  TO                  [8] bytes
                        134, 1, 114, 100, 0, 0, 0, 0,       //  TIMESTAMP           [8] bytes
                1,                                          //  MSG_TYPE            [1] byte 
                1, 0,   100,                                //  HEADER_1            [2 bytes, 1 byte]
                2, 0,   100, 0,                             //  HEADER_2            [2 bytes, 2 bytes]
                4, 0,   100, 0, 0, 0,                       //  HEADER_3            [2 bytes, 4 bytes]
                8, 0,   100, 0, 0, 0, 0, 0, 0, 0,           //  HEADER_4            [2 bytes, 8 bytes]
                32, 0,  97, 97, 97, 97, 97, 97, 97, 97,     //  HEADER_4            [2 bytes, 32 bytes (string)]
                        97, 97, 97, 97, 97, 97, 97, 97,
                        97, 97, 97, 97, 97, 97, 97, 97,
                        97, 97, 97, 97, 97, 97, 97, 97
            };

            object[] defaultSerializerInput = new object[]
            {
                (long)1,
                (long)2,
                (long)1685193094,
                (byte)1,
                (short)1, (byte)100,
                (short)2, (short)100,
                (short)4, (int)100,
                (short)8, (long)100,
                (short)32, STRING_32_BYTES,
            };

            IEnumerable<byte> defaultSerializerResult = defaultSerializerInput.SelectMany(integer => integer.ToByteArray());
            IEnumerable<byte> binarySerializerResult = defaultSerializerInput.SelectMany(data =>
            {
                IEnumerable<byte> result = data switch
                {
                    // Out of some reason BitConverter interprets single byte as 2-byte digit ?? 
                    byte @byte => BitConverter.GetBytes(@byte).Take(1),
                    short @short => BitConverter.GetBytes(@short),
                    int @int => BitConverter.GetBytes(@int),
                    long @long => BitConverter.GetBytes(@long),
                    string @string => System.Text.Encoding.ASCII.GetBytes(@string),
                    _ => throw new ArgumentException($"Argument was invalid. {data.GetType().Name} is not supported.", $"{nameof(data)}", null)
                };

                return result;
            });

            byte[] binaryMessageBuilder = new BinaryMessageBuilder((long)1, (long)2, (long)1685193094, (byte)1)
                .AddHeader("byte", (byte)100)
                .AddHeader("short", (short)100)
                .AddHeader("int", (int)100)
                .AddHeader("long", (long)100)
                .AddHeader("string", STRING_32_BYTES)
                .Serialize();

            bool[] testAssertionConditions = {
                assertBytes.SequenceEqual(binaryMessageBuilder),        // Check against our BinaryBuilder.
                assertBytes.SequenceEqual(defaultSerializerResult),     // Check against our ToByteArray() Extension Method.
                assertBytes.SequenceEqual(binarySerializerResult)       // Check against Built-In BitConverter.
            };

            Assert.That(testAssertionConditions.All(condition => condition), Is.True);
        }

        [Test]
        public void Custom_Bit_Converter_Matches_Results_With_Built_In_One()
        {
            const byte ONE = 1;
            const short THOUSAND = 1_000;
            const int MILLION = 1_000_000;
            const long BILLION = 1_000_000_000;
            const long TRILLION = 1_000_000_000_000;

            // Out of some reason BitConverter interprets single byte as 2-byte digit.
            Assert.Multiple(() =>
            {
                Assert.That(BitConverter.GetBytes(ONE).Take(1).SequenceEqual(ONE.ToByteArray()), Is.True);
                Assert.That(BitConverter.GetBytes(THOUSAND).SequenceEqual(THOUSAND.ToByteArray()), Is.True);
                Assert.That(BitConverter.GetBytes(MILLION).SequenceEqual(MILLION.ToByteArray()), Is.True);
                Assert.That(BitConverter.GetBytes(BILLION).SequenceEqual(BILLION.ToByteArray()), Is.True);
                Assert.That(BitConverter.GetBytes(TRILLION).SequenceEqual(TRILLION.ToByteArray()), Is.True);
            });
        }

        [SetUp]
        public void Setup()
        {
        }
    }
}