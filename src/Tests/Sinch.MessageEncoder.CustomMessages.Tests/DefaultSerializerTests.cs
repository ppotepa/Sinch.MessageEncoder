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
    }
}