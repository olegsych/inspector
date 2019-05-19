using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class PropertyTypeFilterTest
    {
        readonly IFilter<Property> sut;

        // Constructor parameters
        readonly IFilter<Property> previous = Substitute.For<IFilter<Property>>();
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
        }

        public class PropertyType: PropertyTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(propertyType, ((PropertyTypeFilter)sut).PropertyType);
        }

        public class Previous: PropertyTypeFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Property>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get: PropertyTypeFilterTest
        {
            [Fact]
            public void ReturnsPropertiesWithGivenPropertyType() {
                // Arrange
                PropertyInfo propertyInfo = PropertyInfo(MethodAttributes.Static, propertyType);

                var expected = new[] { new Property(propertyInfo), new Property(propertyInfo) };

                var mixed = new[] {
                    new Property(PropertyInfo(MethodAttributes.Static)),
                    expected[0],
                    new Property(PropertyInfo(MethodAttributes.Static)),
                    expected[1],
                    new Property(PropertyInfo(MethodAttributes.Static))
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Property> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
