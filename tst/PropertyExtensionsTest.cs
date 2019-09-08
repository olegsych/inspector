using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests of extension methods that return <see cref="Property"/> and <see cref="Property{T}"/>.
    /// </summary>
    [Collection(nameof(PropertyExtensionsTest))]
    public class PropertyExtensionsTest: SelectorFixture<Property>
    {
        // Method parameters
        protected readonly Type propertyType = typeof(PropertyValue);
        protected readonly string propertyName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Property selected;
        protected IEnumerable<Property> selection;

        public PropertyExtensionsTest() {
            selected = new Property(typeof(TestType).GetProperty(nameof(TestType.Property)), instance);
            select.Invoke(Arg.Do<IEnumerable<Property>>(p => selection = p)).Returns(selected);
        }

        protected static void VerifyGenericProperty<T>(Property selected, Property<T> generic) {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        internal static MemberNameFilter<Property, PropertyInfo> VerifyFilter(IEnumerable<Property> selection, string propertyName) {
            var filter = Assert.IsType<MemberNameFilter<Property, PropertyInfo>>(selection);
            Assert.Equal(propertyName, filter.MemberName);
            return filter;
        }

        internal static PropertyTypeFilter VerifyFilter(IEnumerable<Property> selection, Type expectedPropertyType) {
            var filter = Assert.IsType<PropertyTypeFilter>(selection);
            Assert.Equal(expectedPropertyType, filter.PropertyType);
            return filter;
        }

        protected class TestType
        {
            public PropertyValue Property { get; set; } = new PropertyValue();
        }

        protected class PropertyValue { }

        public class ObjectExtension: PropertyExtensionsTest
        {
            [Fact]
            public void ReturnsSinglePropertyInGivenType() {
                Assert.Same(selected, instance.Property());

                VerifyMembers(selection, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenName() {
                Assert.Same(selected, instance.Property(propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                VerifyMembers(named.Source, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenType() {
                Assert.Same(selected, instance.Property(propertyType));

                PropertyTypeFilter named = VerifyFilter(selection, propertyType);
                VerifyMembers(named.Source, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenTypeAndName() {
                Assert.Same(selected, instance.Property(propertyType, propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Source, propertyType);
                VerifyMembers(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericPropertyOfGivenType() {
                Property<PropertyValue> generic = instance.Property<PropertyValue>();

                VerifyGenericProperty(selected, generic);
                PropertyTypeFilter typed = VerifyFilter(selection, typeof(PropertyValue));
                VerifyMembers(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenTypeAndName() {
                Property<PropertyValue> generic = instance.Property<PropertyValue>(propertyName);

                VerifyGenericProperty(selected, generic);
                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Source, typeof(PropertyValue));
                VerifyMembers(typed.Source, instance);
            }

            static void VerifyMembers(IEnumerable<Property> filter, object instance) {
                var properties = Assert.IsType<Members<PropertyInfo, Property>>(filter);
                Assert.Same(instance, properties.Instance);
            }
        }
    }
}
