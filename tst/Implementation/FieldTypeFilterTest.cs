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
        readonly IEnumerable<Field> source = Substitute.For<IEnumerable<Field>>();
        readonly Type fieldType = Type();

        public FieldTypeFilterTest() =>
            sut = new FieldTypeFilter(source, fieldType);

        public class Constructor: FieldTypeFilterTest
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new FieldTypeFilter(null!, fieldType));
                Assert.Equal("source", thrown.ParamName);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionIfFieldTypeArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new FieldTypeFilter(source, null!));
                Assert.Equal("fieldType", thrown.ParamName);
            }

            [Fact]
            public void PassesSourceToBaseConstructor() =>
                Assert.Same(source, sut.Source);
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

                ConfiguredCall arrange = source.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }
        }
    }
}
