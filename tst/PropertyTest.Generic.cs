using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class GenericPropertyTest
    {
        readonly Property<PropertyType> sut;

        // Constructor parameters
        readonly Property property;

        // Test fixture
        readonly InstanceType instance = new InstanceType();

        public GenericPropertyTest() {
            PropertyInfo info = typeof(InstanceType).GetProperties().Single(_ => _.PropertyType == typeof(PropertyType));
            property = new Property(info, instance);

            sut = new Property<PropertyType>(property);
        }

        public class Constructor: GenericPropertyTest
        {
            [Fact]
            public void InitializesBaseWithGivenArgument() {
                Property @base = sut;
                Assert.Same(property.Info, @base.Info);
                Assert.Same(property.Instance, @base.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenPropertyIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Property<PropertyType>(null!));
                Assert.Equal("property", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInfoDoesNotHaveExpectedPropertyType() {
                PropertyInfo unexpected = typeof(InstanceType).GetProperties().Single(_ => _.PropertyType != typeof(PropertyType));
                var thrown = Assert.Throws<ArgumentException>(() => new Property<PropertyType>(new Property(unexpected, new InstanceType())));
                Assert.Equal("property", thrown.ParamName);
                Assert.StartsWith($"Property type {unexpected.PropertyType} doesn't match expected {typeof(PropertyType)}.", thrown.Message);
            }
        }

        public class Get: GenericPropertyTest
        {
            [Fact]
            public void GetsPropertyValue() {
                PropertyType? value = sut.Get();
                Assert.Same(instance.Property, value);
            }
        }

        public class Set: GenericPropertyTest
        {
            [Fact]
            public void SetsPropertyValue() {
                var value = new PropertyType();
                sut.Set(value);
                Assert.Same(value, instance.Property);
            }
        }

        public class Value: GenericPropertyTest
        {
            [Fact]
            public void GetsPropertyValue() {
                PropertyType? value = sut.Value;
                Assert.Same(instance.Property, value);
            }

            [Fact]
            public void SetsPropertyValue() {
                var value = new PropertyType();
                sut.Value = value;
                Assert.Same(value, instance.Property);
            }
        }

        public class ImplicitOperatorT: GenericPropertyTest
        {
            [Fact]
            public void ImplicitlyConvertsPropertyToValueType() {
                PropertyType? value = sut;
                Assert.Same(value, instance.Property);
            }

            [Fact]
            public void ConvertsNullPropertyToDefaultValueToSupportImplicitTypeConversionRules() {
                Property<PropertyType>? @null = null;
                PropertyType? value = @null;
                Assert.Null(value);
            }
        }

        class InstanceType
        {
            public PropertyType Property { get; set; } = new PropertyType();
            public string AnotherProperty { get; set; } = string.Empty;
        }

        class PropertyType { }
    }
}
