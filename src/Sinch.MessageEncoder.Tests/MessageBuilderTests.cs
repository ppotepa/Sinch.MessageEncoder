using NUnit.Framework;
using Sinch.MessageEncoder.Extensions;
using Sinch.MessageEncoder.Messages;
using Sinch.MessageEncoder.Messages.Default.Text;
using Sinch.MessageEncoder.PoC.Builders;
using Sinch.MessageEncoder.Tests.CustomMessages;
using System;
using System.Linq;

// ReSharper disable InconsistentNaming
namespace Sinch.MessageEncoder.Tests
{
    public class MessageBuilderTests
    {
        const long BILLION = 1_000_000_000;
        const int MILLION = 1_000_000;
        const byte ONE = 1;
        private const string STRING_32_BYTES = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        const short THOUSAND = 1_000;
        const long TRILLION = 1_000_000_000_000;

        private readonly byte[] assertBytes =
        {
            1, 0, 0, 0, 0, 0, 0, 0,                     //  FROM                [8] bytes - translates to [1]
            2, 0, 0, 0, 0, 0, 0, 0,                     //  TO                  [8] bytes - translates to [2]
            134, 1, 114, 100, 0, 0, 0, 0,               //  TIMESTAMP           [8] bytes - translates to { 01.01.0001 00:02:48 }
            100,                                          //  MSG_TYPE            [1] byte  - translates to [1] 
            57, 0, 0, 0, 0, 0, 0, 0,                    //  HEADERS_LENGTH      [8] bytes -translates to  [57]
            1, 0,   100,                                //  HEADER_1            [2 bytes, 1 byte] - translates to - 100 as (byte)
            2, 0,   100, 0,                             //  HEADER_2            [2 bytes, 2 bytes] - translates to - 100 as (short)
            4, 0,   100, 0, 0, 0,                       //  HEADER_3            [2 bytes, 4 bytes] - translates to - 100 as (int)
            8, 0,   100, 0, 0, 0, 0, 0, 0, 0,           //  HEADER_4            [2 bytes, 8 bytes] - translates to - 100 as (long)
            32, 0,  97, 97, 97, 97, 97, 97, 97, 97,     //  HEADER_5            [2 bytes, 32 bytes (string)] - translates to string "a..."[32]
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
            0, 0, 0, 0, 0, 0, 0, 0                      //  HEADERS_LENGTH      [8] bytes - translates to [0] 
        };

        private readonly object[] defaultSerializerInput = {
            (long)1,
            (long)2,
            (long)1685193094,
            (byte)100,
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
            //var binary = new BinaryMessageBuilder(1, 2, 1685193094, 1, 0).Serialize().ToArray();
            var binary = MessageBuilder<DefaultTextMessageHeaders, DefaultTextMessagePayload>.CreateBuilder()
                .From(1)
                .To(2)
                .Timestamp(1685193094)
                .MsgType(1)
                .EndHeaders()
                .GetBinary();

            Assert.That(assertHeaderBytes.SequenceEqual(binary), Is.True);
        }

        [Test]
        public void Binary_Serializer_Serializes_Basic_Headers_Correctly_With_Additional_Headers()
        {
            var defaultSerializerResult = defaultSerializerInput.SelectMany(integer => integer.ToByteArray()).ToArray();
            var binarySerializerResult = defaultSerializerInput.SelectMany(ObjectExtensions.GetBytes).ToArray();

            var builderMessageBinary = MessageBuilder<TestMessageHeader, TestMessagePayload>.CreateBuilder()
                .From(1)
                .To(2)
                .Timestamp(1685193094)
                .MsgType(100)
                .AddHeader("byte", (byte)100)
                .AddHeader("short", (short)100)
                .AddHeader("int", (int)100)
                .AddHeader("long", (long)100)
                .AddHeader("string", STRING_32_BYTES)
                .EndHeaders()
                .GetBinary();

            Console.WriteLine($"defaultSerializerResult: {defaultSerializerResult.Length} {string.Join(",", defaultSerializerResult)}");
            Console.WriteLine($"binarySerializerResult:  {binarySerializerResult.Length} {string.Join(",", binarySerializerResult)}");
            Console.WriteLine($"builderMessageBinary:    {builderMessageBinary.Length} {string.Join(",", builderMessageBinary)}");

            bool[] testAssertionConditions = {
                assertBytes.SequenceEqual(builderMessageBinary),               // Check against BinaryBuilder.
                assertBytes.SequenceEqual(defaultSerializerResult),     // Check against ToByteArray() Extension Method.
                assertBytes.SequenceEqual(binarySerializerResult)       // Check against Built-In BitConverter.
            };

            Assert.That(testAssertionConditions.All(condition => condition), Is.True);
        }

        [Test]
        public void Binary_Serializer_Throws_An_Exception_When_Header_Is_Missing()
        {
            var builderMessageBinary = MessageBuilder<DefaultTextMessageHeaders, DefaultTextMessagePayload>
                .CreateBuilder()
                .From(1)
                .To(2)
                .Timestamp(1685193094)
                .MsgType(1)
                .AddHeader("recipient-name", "AAAAA");

            Assert.Throws<InvalidHeaderNameException>(() => builderMessageBinary.AddHeader<string>("bad-header-name", null));
        }

        [Test]
        public void Binary_Serializer_Throws_An_Exception_When_Headers_Count_Is_To_High()
        {
            var builderMessageBinary = MessageBuilder<DefaultTextMessageHeaders, DefaultTextMessagePayload>
                .CreateBuilder()
                .From(1)
                .To(2)
                .Timestamp(1685193094)
                .MsgType(1)
                .AddHeader("recipient-name", "AAAAA")
                .AddHeader("sender-name", "AAAAA")
                .AddHeader("is-message-unread", "AAAAA");

            Assert.Throws<HeadersCountExceededException>(() => builderMessageBinary.AddHeader<string>("excessive-header-name", null));
        }

        [Test]
        public void Binary_Serializer_Throws_An_Exception_When_Payload_Properties_Count_Is_To_High()
        {
            var builderMessageBinary = MessageBuilder<DefaultTextMessageHeaders, DefaultTextMessagePayload>
                .CreateBuilder()
                .From(1)
                .To(2)
                .Timestamp(1685193094)
                .MsgType(1)
                .AddHeader("recipient-name", "AAAAA")
                .AddHeader("sender-name", "AAAAA")
                .AddHeader("is-message-unread", "AAAAA")
                .EndHeaders()
                .AddPayloadProperty("TextMessageBody", "Some text.");

            Assert.Throws<PayloadPropertiesCountExceeded>(() => builderMessageBinary.AddPayloadProperty<string>("ExcessivePropertyName", null));
        }

        [Test]
        public void BinarySerializer_Works_Against_DefaultTextMessage()
        {
            byte[] builderMessageBinary = MessageBuilder<DefaultTextMessageHeaders, DefaultTextMessagePayload>.CreateBuilder()
                .From(1)
                .To(2)
                .Timestamp(1685193094)
                .MsgType(1)
                .AddHeader("recipient-name", "AAAAA")
                .AddHeader("sender-name", "AAAAA")
                .AddHeader("is-message-unread", true)
                .EndHeaders()
                .AddPayloadProperty("TextMessageBody", "AAAAA")
                .GetBinary();

            var myTestMessage = new DefaultTextMessage
            {
                Headers = new DefaultTextMessageHeaders
                {
                    From = 1,
                    To = 2,
                    MessageType = 1,
                    Timestamp = 1685193094,
                    IsMessageUnread = true,
                    RecipientName = "AAAAA",
                    SenderName = "AAAAA"
                },

                Payload = new DefaultTextMessagePayload
                {
                    TextMessageBody = "AAAAA"
                }
            };

            var myTestMessageArray = Message.ToBinary(myTestMessage).ToArray();
            //var myTestMessageInstance = Message.FromBytes(myTestMessageArray);

            Console.WriteLine($"BuilderMessage Length {builderMessageBinary.Length} {string.Join(",", builderMessageBinary.ToArray())}");
            Console.WriteLine($"Message.Create Length {myTestMessageArray.Length} {string.Join(",", myTestMessageArray)}");

            var myTestMessageBinary = Message.ToBinary(myTestMessage).ToArray();
            Assert.That(builderMessageBinary.SequenceEqual(myTestMessageBinary), Is.True);
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