using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class IScopeExtensionsTest
    {
        public class FieldMethod : FieldFixture
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();
            readonly string fieldName = Guid.NewGuid().ToString();
            readonly Type fieldType = typeof(FieldValue);

            // Shared test fixture
            readonly TestType instance = new TestType();
            readonly Field field;

            public FieldMethod() =>
                field = new Field(typeof(TestType).GetField(nameof(TestType.Field)), instance);

            [Fact]
            public void ReturnsSingleFieldInGivenScope() {
                selector.Invoke(scope, null, null).Returns(field);
                Assert.Same(field, scope.Field());
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                selector.Invoke(scope, null, fieldName).Returns(field);
                Assert.Same(field, scope.Field(fieldName));
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                selector.Invoke(scope, fieldType, null).Returns(field);
                Assert.Same(field, scope.Field(fieldType));
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                selector.Invoke(scope, fieldType, fieldName).Returns(field);
                Assert.Same(field, scope.Field(fieldType, fieldName));
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenType() {
                selector.Invoke(scope, typeof(FieldValue), null).Returns(field);

                Field<FieldValue> genericField = scope.Field<FieldValue>();

                Assert.Same(field.Info, genericField.Info);
                Assert.Same(field.Instance, genericField.Instance);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                selector.Invoke(scope, typeof(FieldValue), fieldName).Returns(field);

                Field<FieldValue> genericField = scope.Field<FieldValue>(fieldName);

                Assert.Same(field.Info, genericField.Info);
                Assert.Same(field.Instance, genericField.Instance);
            }

            class TestType
            {
                public FieldValue Field = new FieldValue();
            }

            class FieldValue { }
        }
    }
}
