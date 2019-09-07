using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class PropertyTypeFilterTest
    {
        readonly Filter<Property> sut;

        // Constructor parameters
        readonly IEnumerable<Property> previous = Substitute.For<IEnumerable<Property>>();
        readonly Type propertyType = Type();

        public PropertyTypeFilterTest() =>
            sut = new PropertyTypeFilter(previous, propertyType);

        public class Constructor: PropertyTypeFilterTest
        {
            [Fact]
            public void ThrowsArgumentNullExceptionIfPropertiesArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new PropertyTypeFilter(null, propertyType));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionIfPropertyTypeArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new PropertyTypeFilter(previous, null));
                Assert.Equal("propertyType", thrown.ParamName);
            }

            [Fact]
            public void PassesPreviousToBaseConstructor() =>
                Assert.Same(previous, sut.Previous);
        }

        public class PropertyType: PropertyTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(propertyType, ((PropertyTypeFilter)sut).PropertyType);
        }

        public class GetEnumerator: PropertyTypeFilterTest
        {
            [Fact]
            public void ReturnsPropertiesWithGivenPropertyType() {
                PropertyInfo propertyInfo = PropertyInfo(MethodAttributes.Static, propertyType);

                var expected = new[] { new Property(propertyInfo), new Property(propertyInfo) };

                IEnumerable<Property> mixed = new[] {
                    new Property(PropertyInfo(MethodAttributes.Static)),
                    expected[0],
                    new Property(PropertyInfo(MethodAttributes.Static)),
                    expected[1],
                    new Property(PropertyInfo(MethodAttributes.Static))
                };

                ConfiguredCall arrange = previous.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }
        }
    }
}
