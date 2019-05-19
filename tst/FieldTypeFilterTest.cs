using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class FieldTypeFilterTest
    {
        readonly IFilter<Field> sut;

        // Constructor parameters
        readonly IFilter<Field> previous = Substitute.For<IFilter<Field>>();
        readonly Type fieldType = Type();

        public FieldTypeFilterTest() =>
            sut = new FieldTypeFilter(previous, fieldType);

        public class Constructor: FieldTypeFilterTest
        {
            [Fact]
            public void ThrowsArgumentNullExceptionIfFieldsArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new FieldTypeFilter(null, fieldType));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionIfFieldTypeArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new FieldTypeFilter(previous, null));
                Assert.Equal("fieldType", thrown.ParamName);
            }
        }

        public class FieldType: FieldTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(fieldType, ((FieldTypeFilter)sut).FieldType);
        }

        public class Previous: FieldTypeFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Field>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get: FieldTypeFilterTest
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

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Field> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
