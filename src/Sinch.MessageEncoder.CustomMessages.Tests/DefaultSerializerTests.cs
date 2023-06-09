using NUnit.Framework;
using Sinch.MessageEncoder.CustomMessages.Tests.CustomMessages;
using Sinch.MessageEncoder.CustomMessages.Tests.CustomMessages.InvalidOrder;
using Sinch.MessageEncoder.CustomMessages.Tests.CustomMessages.ValidOrder;
using Sinch.MessageEncoder.Exceptions;
using Sinch.MessageEncoder.Factories.Serialization;
using Sinch.MessageEncoder.Metadata.Serialization;
using Sinch.MessageEncoder.Serializers;
using System;
using System.Linq;

// ReSharper disable InconsistentNaming
namespace Sinch.MessageEncoder.CustomMessages.Tests
{
    public class DefaultSerializerTests
    {
        private readonly byte[] assertBytes =
        {
            1, 0, 0, 0, 0, 0, 0, 0,                     //  FROM                [8] bytes - translates to [1]
            2, 0, 0, 0, 0, 0, 0, 0,                     //  TO                  [8] bytes - translates to [2]
            134, 1, 114, 100, 0, 0, 0, 0,               //  TIMESTAMP           [8] bytes - translates to { 01.01.0001 00:02:48 }
            200,                                        //  MSG_TYPE            [1] byte  - translates to [1] 
            57, 0, 0, 0, 0, 0, 0, 0,                    //  HEADERS_LENGTH      [8] bytes -translates to  [57]
            1, 0,   100,                                //  HEADER_1            [2 bytes, 1 byte] - translates to - 100 as (byte)
            2, 0,   100, 0,                             //  HEADER_2            [2 bytes, 2 bytes] - translates to - 100 as (short)
            4, 0,   100, 0, 0, 0,                       //  HEADER_3            [2 bytes, 4 bytes] - translates to - 100 as (int)
            8, 0,   100, 0, 0, 0, 0, 0, 0, 0,           //  HEADER_4            [2 bytes, 8 bytes] - translates to - 100 as (long)
            32, 0,  97, 97, 97, 97, 97, 97, 97, 97,     //  HEADER_5            [2 bytes, 32 bytes (string)] - translates to string "a..."[32]
            97, 97, 97, 97, 97, 97, 97, 97,
            97, 97, 97, 97, 97, 97, 97, 97,
            97, 97, 97, 97, 97, 97, 97, 97,
            4, 0, 0, 0, 128, 63,
            8, 0, 0,0,0,0,0,0,240, 63,
            1, 0, 1

        };

        [Test]
        public void Serialization_Metadata_Throws_An_Exception_If_PropertyOrder_Attributes_Are_Duplicated()
        {
            Assert.Throws<InvalidSerializationOrderException>(() => SerializationMetadata.Create(typeof(InvalidOrderHeader)));
        }

        [Test]
        public void Serialization_Metadata_Return_Single_Element_Array_For_Valid_Order_message()
        {
            var result = SerializationMetadata.Create(typeof(ValidOrderHeader));

            var resultLengthIsOne = result.Length is 1;
            var resultPropertyLengthIsNotNull = result[0].PropertyInfo is not null;
            var resultPropertyLengthIsTypeOfString = result[0].PropertyInfo!.PropertyType == typeof(string);

            var conditions = new bool[]
            {
                resultLengthIsOne,
                resultPropertyLengthIsNotNull,
                resultPropertyLengthIsTypeOfString
            };

            Assert.That(() => conditions.All(condition => condition is true));
        }

        [Test]
        public void Serialization_Metadata_Throws_An_Exception_If_Passed_Type_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => SerializationMetadata.Create(null));
        }

        [Test]
        public void Creating_A_Serializer_For_Type_That_Is_Nor_Header_Or_Payload_Results_In_Exception()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(
                    () => SerializersFactory.CreateSerializer<IPayloadSerializer>(null));

                Assert.Throws<ArgumentException>(
                    () => SerializersFactory.CreateSerializer<IPayloadSerializer>(typeof(NotAPayload)));

                Assert.Throws<ArgumentException>(
                    () => SerializersFactory.CreateSerializer<IHeadersSerializer>(typeof(NotAHeader)));
            });
        }

        [SetUp]
        public void Setup()
        {
        }
    }
}