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
        readonly IEnumerable<Property> source = Substitute.For<IEnumerable<Property>>();
        readonly Type propertyType = Type();

        public PropertyTypeFilterTest() =>
            sut = new PropertyTypeFilter(source, propertyType);

        public class Constructor: PropertyTypeFilterTest
        {
            [Fact]
            public void ThrowsArgumentNullExceptionIfSourceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new PropertyTypeFilter(null!, propertyType));
                Assert.Equal("source", thrown.ParamName);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionIfPropertyTypeArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new PropertyTypeFilter(source, null!));
                Assert.Equal("propertyType", thrown.ParamName);
            }

            [Fact]
            public void PassesSourceToBaseConstructor() =>
                Assert.Same(source, sut.Source);
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

                Property[] expected = { new Property(propertyInfo), new Property(propertyInfo) };

                IEnumerable<Property> mixed = new[] {
                    new Property(PropertyInfo(MethodAttributes.Static)),
                    expected[0],
                    new Property(PropertyInfo(MethodAttributes.Static)),
                    expected[1],
                    new Property(PropertyInfo(MethodAttributes.Static))
                };

                ConfiguredCall arrange = source.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }
        }
    }
}
