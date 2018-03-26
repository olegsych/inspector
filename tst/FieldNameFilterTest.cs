using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class FieldNameFilterTest
    {
        readonly IFilter<Field> sut;

        // Constructor parameters
        readonly IFilter<Field> previous = Substitute.For<IFilter<Field>>();
        readonly string fieldName = Guid.NewGuid().ToString();

        public FieldNameFilterTest() =>
            sut = new FieldNameFilter(previous, fieldName);

        public class Constructor : FieldNameFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new FieldNameFilter(null, fieldName));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFieldNameIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new FieldNameFilter(previous, null));
                Assert.Equal("fieldName", thrown.ParamName);
            }
        }

        public class FieldName : FieldNameFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(fieldName, ((FieldNameFilter)sut).FieldName);
        }

        public class Previous : FieldNameFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Field>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get : FieldNameFilterTest
        {
            [Fact]
            public void ReturnsFieldsWithGivenName() {
                // Arrange
                FieldInfo fieldInfo = FieldInfo(FieldAttributes.Static, fieldName);

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
