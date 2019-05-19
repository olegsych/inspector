using System.Reflection;
using Xunit;

namespace Inspector
{
    public class PropertyTest
    {
        readonly Property sut;

        // Constructor parameters
        readonly PropertyInfo property = typeof(TestType).GetProperty(nameof(TestType.Property));
        readonly PropertyInfo staticProperty = typeof(TestType).GetProperty(nameof(TestType.StaticProperty));
        readonly object instance = new TestType();

        public PropertyTest() =>
            sut = new Property(property, instance);

        public class Constructor: PropertyTest
        {
            [Fact]
            public void InitializesBaseTypeWithGivenPropertyInfoAndInstance() {
                Member<PropertyInfo> member = sut;

                Assert.Same(property, member.Info);
                Assert.Same(instance, member.Instance);
            }
        }

        public class Create: PropertyTest
        {
            [Fact]
            public void ReturnsFieldWithGivenPropertyInfoAndInstance() {
                Property actual = Property.Create(property, instance);

                Assert.Same(property, actual.Info);
                Assert.Same(instance, actual.Instance);
            }
        }

        public class Get: PropertyTest
        {
            [Fact]
            public void GetsPropertyValue() {
                var expected = new TestValue();
                ((TestType)instance).Property = expected;

                object actual = sut.Get();

                Assert.Same(expected, actual);
            }
        }

        public class IsStatic: PropertyTest
        {
            [Fact]
            public void ReturnsTrueForStaticProperty() =>
                Assert.True(new Property(staticProperty, null).IsStatic);

            [Fact]
            public void ReturnsFalseForInstanceProperty() =>
                Assert.False(new Property(property, instance).IsStatic);
        }

        public class Set: PropertyTest
        {
            [Fact]
            public void SetsPropertyValue() {
                object expected = new TestValue();
                sut.Set(expected);
                Assert.Same(expected, ((TestType)instance).Property);
            }
        }

        public class Value: PropertyTest
        {
            [Fact]
            public void GetsPropertyValue() {
                var expected = new TestValue();
                ((TestType)instance).Property = expected;

                object actual = sut.Value;

                Assert.Same(expected, actual);
            }

            [Fact]
            public void SetsPropertyValue() {
                object expected = new TestValue();
                sut.Value = expected;
                Assert.Same(expected, ((TestType)instance).Property);
            }
        }

        class TestType
        {
            public TestValue Property { get; set; }
            public static TestValue StaticProperty { get; set; }
        }

        class TestValue { }
    }
}
