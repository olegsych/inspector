using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests of extension methods that return <see cref="Property"/> and <see cref="Property{T}"/>.
    /// </summary>
    [Collection(nameof(PropertyExtensionsTest))]
    public class PropertyExtensionsTest : SelectorFixture<Property>
    {
        // Method parameters
        protected readonly Type propertyType = typeof(PropertyValue);
        protected readonly string propertyName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Property selected;
        protected IFilter<Property> selection;

        public PropertyExtensionsTest() {
            selected = new Property(typeof(TestType).GetProperty(nameof(TestType.Property)), instance);
            select.Invoke(Arg.Do<IFilter<Property>>(p => selection = p)).Returns(selected);
        }

        protected static void VerifyGenericProperty<T>(Property selected, Property<T> generic) {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        internal static MemberNameFilter<Property, PropertyInfo> VerifyFilter(IFilter<Property> selection, string propertyName) {
            var filter = Assert.IsType<MemberNameFilter<Property, PropertyInfo>>(selection);
            Assert.Equal(propertyName, filter.MemberName);
            return filter;
        }

        internal static PropertyTypeFilter VerifyFilter(IFilter<Property> selection, Type expectedPropertyType) {
            var filter = Assert.IsType<PropertyTypeFilter>(selection);
            Assert.Equal(expectedPropertyType, filter.PropertyType);
            return filter;
        }

        protected class TestType
        {
            public PropertyValue Property { get; set; } = new PropertyValue();
        }

        protected class PropertyValue { }

        public class IScopeExtension : PropertyExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void ReturnsSinglePropertyInGivenScope() {
                Assert.Same(selected, scope.Property());

                Assert.Same(scope, selection);
            }

            [Fact]
            public void ReturnsPropertyWithGivenName() {
                Assert.Same(selected, scope.Property(propertyName));

                MemberNameFilter<Property, PropertyInfo> filter = VerifyFilter(selection, propertyName);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsPropertyWithGivenType() {
                Assert.Same(selected, scope.Property(propertyType));

                PropertyTypeFilter filter = VerifyFilter(selection, propertyType);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsPropertyWithGivenTypeAndName() {
                Assert.Same(selected, scope.Property(propertyType, propertyName));

                MemberNameFilter<Property, PropertyInfo> nameFilter = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typeFilter = VerifyFilter(nameFilter.Previous, propertyType);
                Assert.Same(scope, typeFilter.Previous);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenType() {
                Property<PropertyValue> generic = scope.Property<PropertyValue>();

                VerifyGenericProperty(selected, generic);
                PropertyTypeFilter typeFilter = VerifyFilter(selection, typeof(PropertyValue));
                Assert.Same(scope, typeFilter.Previous);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenTypeAndName() {
                Property<PropertyValue> generic = scope.Property<PropertyValue>(propertyName);

                VerifyGenericProperty(selected, generic);
                MemberNameFilter<Property, PropertyInfo> nameFilter = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typeFilter = VerifyFilter(nameFilter.Previous, propertyType);
                Assert.Same(scope, typeFilter.Previous);
            }
        }

        public class ObjectExtension : PropertyExtensionsTest
        {
            [Fact]
            public void ReturnsSinglePropertyInGivenType() {
                Assert.Same(selected, instance.Property());

                VerifyScope(selection, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenName() {
                Assert.Same(selected, instance.Property(propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenType() {
                Assert.Same(selected, instance.Property(propertyType));

                PropertyTypeFilter named = VerifyFilter(selection, propertyType);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenTypeAndName() {
                Assert.Same(selected, instance.Property(propertyType, propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Previous, propertyType);
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericPropertyOfGivenType() {
                Property<PropertyValue> generic = instance.Property<PropertyValue>();

                VerifyGenericProperty(selected, generic);
                PropertyTypeFilter typed = VerifyFilter(selection, typeof(PropertyValue));
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenTypeAndName() {
                Property<PropertyValue> generic = instance.Property<PropertyValue>(propertyName);

                VerifyGenericProperty(selected, generic);
                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Previous, typeof(PropertyValue));
                VerifyScope(typed.Previous, instance);
            }

            static void VerifyScope(IFilter<Property> filter, object instance) {
                var scope = Assert.IsType<InstanceScope>(filter);
                Assert.Same(instance, scope.Instance);
            }
        }

        public class TypeExtension : PropertyExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSinglePropertyInGivenType() {
                Assert.Same(selected, testType.Property());

                VerifyScope(selection, testType);
            }

            [Fact]
            public void ReturnsPropertyWithGivenName() {
                Assert.Same(selected, testType.Property(propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                VerifyScope(named.Previous, testType);
            }

            [Fact]
            public void ReturnsPropertyWithGivenType() {
                Assert.Same(selected, testType.Property(propertyType));

                PropertyTypeFilter typed = VerifyFilter(selection, propertyType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsPropertyWithGivenTypeAndName() {
                Assert.Same(selected, testType.Property(propertyType, propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Previous, propertyType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsGenericPropertyOfGivenType() {
                Property<PropertyValue> generic = testType.Property<PropertyValue>();

                VerifyGenericProperty(selected, generic);
                PropertyTypeFilter typed = VerifyFilter(selection, typeof(PropertyValue));
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenTypeAndName() {
                Property<PropertyValue> generic = testType.Property<PropertyValue>(propertyName);

                VerifyGenericProperty(selected, generic);
                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Previous, typeof(PropertyValue));
                VerifyScope(typed.Previous, testType);
            }

            static void VerifyScope(IFilter<Property> selection, Type expected) {
                var scope = Assert.IsType<StaticScope>(selection);
                Assert.Same(expected, scope.Type);
            }
        }
    }
}
