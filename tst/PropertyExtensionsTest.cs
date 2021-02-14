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
        protected IEnumerable<Property>? selection;

        public PropertyExtensionsTest() {
            selected = new Property(typeof(TestType).GetProperty(nameof(TestType.Property))!, instance);
            object arrange = select.Invoke(Arg.Do<IEnumerable<Property>>(p => selection = p)).Returns(selected);
        }

        protected static void VerifyGenericProperty<T>(Property selected, Property<T> generic) {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        internal static MemberNameFilter<Property, PropertyInfo> VerifyFilter(IEnumerable<Property>? selection, string propertyName) {
            Assert.NotNull(selection);
            var filter = (MemberNameFilter<Property, PropertyInfo>)selection!;
            Assert.Equal(propertyName, filter.MemberName);
            return filter;
        }

        internal static PropertyTypeFilter VerifyFilter(IEnumerable<Property>? selection, Type expectedPropertyType) {
            Assert.NotNull(selection);
            var filter = (PropertyTypeFilter)selection!;
            Assert.Equal(expectedPropertyType, filter.PropertyType);
            return filter;
        }

        protected class TestType
        {
            public PropertyValue Property { get; set; } = new PropertyValue();
        }

        protected class PropertyValue { }
    }
}
