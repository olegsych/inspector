using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class TypedFieldFilterTest
    {
        readonly IFilter<Field> sut;

        // Constructor parameters
        readonly IFilter<Field> fields = Substitute.For<IFilter<Field>>();
        readonly Type fieldType = Type();

        public TypedFieldFilterTest() =>
            sut = new TypedFieldFilter(fields, fieldType);

        public class Constructor : TypedFieldFilterTest
        {
            [Fact]
            public void ThrowsArgumentNullExceptionIfFieldsArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new TypedFieldFilter(null, fieldType));
                Assert.Equal("fields", thrown.ParamName);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionIfFieldTypeArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new TypedFieldFilter(fields, null));
                Assert.Equal("fieldType", thrown.ParamName);
            }
        }

        public class Get : TypedFieldFilterTest
        {
            [Fact]
            public void ReturnsFieldsWithGivenFieldType() {
                // Arrange
                FieldInfo fieldInfo = FieldInfo(FieldAttributes.Static, fieldType);

                var expected = new[] { new Field(fieldInfo), new Field(fieldInfo) };

                var mixed = new[] {
                    new Field(FieldInfo(FieldAttributes.Static)),
                    expected[0],
                    new Field(FieldInfo(FieldAttributes.Static)),
                    expected[1],
                    new Field(FieldInfo(FieldAttributes.Static))
                };

                fields.Get().Returns(mixed);

                // Act
                IEnumerable<Field> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
