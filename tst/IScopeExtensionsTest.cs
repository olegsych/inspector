using NSubstitute;
using Xunit;

namespace Inspector
{
    public class IScopeExtensionsTest
    {
        public class FieldMethod : FieldExtensionFixture
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void ReturnsSingleFieldInGivenScope() {
                Assert.Same(selected, scope.Field());

                Assert.Same(scope, selection);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, scope.Field(fieldName));

                FieldNameFilter filter = VerifyFilter(selection, fieldName);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, scope.Field(fieldType));

                FieldTypeFilter filter = VerifyFilter(selection, fieldType);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, scope.Field(fieldType, fieldName));

                FieldNameFilter nameFilter = VerifyFilter(selection, fieldName);
                FieldTypeFilter typeFilter = VerifyFilter(nameFilter.Previous, fieldType);
                Assert.Same(scope, typeFilter.Previous);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenType() {
                Field<FieldValue> generic = scope.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typeFilter = VerifyFilter(selection, typeof(FieldValue));
                Assert.Same(scope, typeFilter.Previous);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = scope.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                FieldNameFilter nameFilter = VerifyFilter(selection, fieldName);
                FieldTypeFilter typeFilter = VerifyFilter(nameFilter.Previous, fieldType);
                Assert.Same(scope, typeFilter.Previous);
            }
        }
    }
}
