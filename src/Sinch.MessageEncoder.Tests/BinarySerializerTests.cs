using NUnit.Framework;
using Sinch.MessageEncoder.PoC;
using Sinch.MessageEncoder.PoC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using ObjectExtensions = Sinch.MessageEncoder.Extensions.ObjectExtensions;

// ReSharper disable InconsistentNaming

namespace Sinch.MessageEncoder.Tests
{
    public class Tests
    {
        const byte ONE = 1;
        const short THOUSAND = 1_000;
        const int MILLION = 1_000_000;
        const long BILLION = 1_000_000_000;
        const long TRILLION = 1_000_000_000_000;
        private const string STRING_32_BYTES = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        private readonly byte[] assertBytes =
        {
            1, 0, 0, 0, 0, 0, 0, 0,                     //  FROM                [8] bytes - translates to [1]
            2, 0, 0, 0, 0, 0, 0, 0,                     //  TO                  [8] bytes - translates to [2]
            134, 1, 114, 100, 0, 0, 0, 0,               //  TIMESTAMP           [8] bytes - translates to { 01.01.0001 00:02:48 }
            1,                                          //  MSG_TYPE            [1] byte  - translates to [1] 
            57, 0, 0, 0, 0, 0, 0, 0,                    //  HEADERS_LENGTH      [8] bytes -translates to  [57]
            1, 0,   100,                                //  HEADER_1            [2 bytes, 1 byte] - translates to - 100 as (byte)
            2, 0,   100, 0,                             //  HEADER_2            [2 bytes, 2 bytes] - translates to - 100 as (short)
            4, 0,   100, 0, 0, 0,                       //  HEADER_3            [2 bytes, 4 bytes] - translates to - 100 as (int)
            8, 0,   100, 0, 0, 0, 0, 0, 0, 0,           //  HEADER_4            [2 bytes, 8 bytes] - translates to - 100 as (long)
            32, 0,  97, 97, 97, 97, 97, 97, 97, 97,     //  HEADER_4            [2 bytes, 32 bytes (string)] - translates to string "a..."[32]
            97, 97, 97, 97, 97, 97, 97, 97,
            97, 97, 97, 97, 97, 97, 97, 97,
            97, 97, 97, 97, 97, 97, 97, 97
        };

        readonly byte[] assertHeaderBytes =
        {
            1, 0, 0, 0, 0, 0, 0, 0,                     //  FROM                [8] bytes - translates to [1]
            2, 0, 0, 0, 0, 0, 0, 0,                     //  TO                  [8] bytes - translates to [2]
            134, 1, 114, 100, 0, 0, 0, 0,               //  TIMESTAMP           [8] bytes - translates to { 01.01.0001 00:02:48 }
            1,                                          //  MSG_TYPE            [1] byte  - translates to [1] 
            0, 0, 0, 0, 0, 0, 0, 0                      //  MSG_TYPE            [1] byte  - translates to [1] 
        };

        private readonly object[] defaultSerializerInput = {
            (long)1,
            (long)2,
            (long)1685193094,
            (byte)1,
            (long)57,
            (short)1, (byte)100,
            (short)2, (short)100,
            (short)4, (int)100,
            (short)8, (long)100,
            (short)32, STRING_32_BYTES,
        };

        [Test]
        public void Binary_Serializer_Serializes_Basic_Headers_Correctly()
        {
            byte[] binaryMessageBuilder = new BinaryMessageBuilder(1, 2, 1685193094, 1, 0).Serialize();
            Assert.That(assertHeaderBytes.SequenceEqual(binaryMessageBuilder), Is.True);
        }

        [Test]
        public void Binary_Serializer_Serializes_Basic_Headers_Correctly_With_Additional_Headers()
        {
            IEnumerable<byte> defaultSerializerResult = defaultSerializerInput.SelectMany(integer => integer.ToByteArray());
            IEnumerable<byte> binarySerializerResult = defaultSerializerInput.SelectMany(ObjectExtensions.GetBytes);

            byte[] binaryMessageBuilder = new BinaryMessageBuilder((long)1, (long)2, (long)1685193094, (byte)1, ((1 + 2 + 4 + 8 + 32) + (2 * 5)))
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
            Assert.Multiple(() =>
            {
                // Out of some reason BitConverter interprets single byte as 2-byte digit.
                Assert.That(BitConverter.GetBytes((short)ONE).Take(1).SequenceEqual(ONE.ToByteArray()), Is.True);
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