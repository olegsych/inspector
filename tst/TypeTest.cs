using Xunit;

namespace Inspector
{
    public class TypeTest
    {
        public class New: TypeTest
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
                var instance = Type<TypeWithPublicConstructor>.New(value);
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
                var instance = Type<TypeWithPrivateConstructor>.New(value);
                Assert.Same(value, instance.Property);
            }
        }

        public class Uninitialized: TypeTest
        {
            class TestType
            {
                public readonly int TestField = 42;
            }

            [Fact]
            public void ReturnsUninitializedInstanceOfGivenType() {
                var instance = Type<TestType>.Uninitialized();
                Assert.Equal(0, instance.TestField);
            }
        }
    }
}
