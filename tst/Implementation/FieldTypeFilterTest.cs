using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class FieldTypeFilterTest
    {
        readonly Filter<Field> sut;

        // Constructor parameters
        readonly IEnumerable<Field> previous = Substitute.For<IEnumerable<Field>>();
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

            [Fact]
            public void PassesPreviousToBaseConstructor() =>
                Assert.Same(previous, sut.Previous);
        }

        public class FieldType: FieldTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(fieldType, ((FieldTypeFilter)sut).FieldType);
        }

        public class GetEnumerator: FieldTypeFilterTest
        {
            [Fact]
            public void ReturnsFieldsWithGivenFieldType() {
                FieldInfo fieldInfo = FieldInfo(FieldAttributes.Static, fieldType);

                var expected = new[] { new Field(fieldInfo), new Field(fieldInfo) };

                IEnumerable<Field> mixed = new[] {
                    new Field(FieldInfo(FieldAttributes.Static)),
                    expected[0],
                    new Field(FieldInfo(FieldAttributes.Static)),
                    expected[1],
                    new Field(FieldInfo(FieldAttributes.Static))
                };

                ConfiguredCall arrange = previous.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }
        }
    }
}
