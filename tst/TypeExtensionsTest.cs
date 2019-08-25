using Xunit;

namespace Inspector
{
    public class TypeExtensionsTest
    {
        public class New: TypeExtensionsTest
        {
            class PropertyType { }

            class TypeWithPublicConstructor
            {
                public TypeWithPublicConstructor(PropertyType value) =>
                    Property = value;

                public PropertyType Property { get; }
            }

            [Fact]
            public void ReturnsNewInstanceCreatedByPublicConstructor() {
                var value = new PropertyType();
                var instance = Assert.IsType<TypeWithPublicConstructor>(typeof(TypeWithPublicConstructor).New(value));
                Assert.Same(value, instance.Property);
            }

            class TypeWithPrivateConstructor
            {
                TypeWithPrivateConstructor(PropertyType value) =>
                    Property = value;

                public PropertyType Property { get; }
            }

            [Fact]
            public void ReturnsNewInstanceCreatedByPrivateConstructor() {
                var value = new PropertyType();
                var instance = Assert.IsType<TypeWithPrivateConstructor>(typeof(TypeWithPrivateConstructor).New(value));
                Assert.Same(value, instance.Property);
            }
        }

        public class Uninitialized: TypeExtensionsTest
        {
            class TestType
            {
                public readonly int TestField = 42;
            }

            [Fact]
            public void ReturnsUninitializedInstanceOfGivenType() {
                var instance = Assert.IsType<TestType>(typeof(TestType).Uninitialized());
                Assert.Equal(0, instance.TestField);
            }
        }
    }
}
