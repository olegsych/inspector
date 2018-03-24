using System;
using System.Linq.Expressions;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest
    {
        public class FieldMethod : FieldFixture
        {
            // Method parameters
            readonly object instance = new TestType();
            readonly Type fieldType = typeof(FieldValue);
            readonly string fieldName = Guid.NewGuid().ToString();

            // Test fixture
            readonly Expression<Predicate<InstanceScope>> instanceScope;
            readonly Field expectedField;

            public FieldMethod() {
                instanceScope = scope => instance == scope.Instance;
                expectedField = new Field(typeof(TestType).GetField(nameof(TestType.Field)), instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                selector.Invoke(Arg.Is(instanceScope), fieldType, fieldName).Returns(expectedField);
                Assert.Same(expectedField, instance.Field(fieldType, fieldName));
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                selector.Invoke(Arg.Is(instanceScope), fieldType, null).Returns(expectedField);
                Assert.Same(expectedField, instance.Field(fieldType));
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                selector.Invoke(Arg.Is(instanceScope), null, fieldName).Returns(expectedField);
                Assert.Same(expectedField, instance.Field(fieldName));
            }

            [Fact]
            public void ReturnsSingleFieldInGivenType() {
                selector.Invoke(Arg.Is(instanceScope), null, null).Returns(expectedField);
                Assert.Same(expectedField, instance.Field());
            }

            [Fact]
            public void ReturnsGenericFieldOfGivenType() {
                selector.Invoke(Arg.Is(instanceScope), fieldType, null).Returns(expectedField);

                Field<FieldValue> genericField = instance.Field<FieldValue>();

                Assert.Same(expectedField.Info, genericField.Info);
                Assert.Same(expectedField.Instance, genericField.Instance);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                selector.Invoke(Arg.Is(instanceScope), fieldType, fieldName).Returns(expectedField);

                Field<FieldValue> genericField = instance.Field<FieldValue>(fieldName);

                Assert.Same(expectedField.Info, genericField.Info);
                Assert.Same(expectedField.Instance, genericField.Instance);
            }

            class TestType
            {
                public FieldValue Field = new FieldValue();
            }

            class FieldValue { }
        }
    }
}
