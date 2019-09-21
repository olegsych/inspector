using System;
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

            class TestException: Exception { }

            [Fact]
            public void ReturnsNewInstanceCreatedByPrivateConstructor() {
                var value = new PropertyType();
                var instance = Type<TypeWithPrivateConstructor>.New(value);
                Assert.Same(value, instance.Property);
            }

            class TypeWithThrowingConstructor
            {
                TypeWithThrowingConstructor() => throw new TestException();
            }

            [Fact]
            public void UnwrapsOriginalExceptionThrownByConstructor() =>
                Assert.Throws<TestException>(() => Type<TypeWithThrowingConstructor>.New());
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
