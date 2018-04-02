using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest
    {
        public class FieldMethod : FieldExtensionFixture
        {
            [Fact]
            public void ReturnsSingleFieldInGivenType() {
                Assert.Same(selected, instance.Field());

                VerifyScope(selection, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, instance.Field(fieldType));

                FieldTypeFilter named = VerifyFilter(selection, fieldType);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, instance.Field(fieldName));

                FieldNameFilter named = VerifyFilter(selection, fieldName);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, instance.Field(fieldType, fieldName));

                FieldNameFilter named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Previous, fieldType);
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericFieldOfGivenType() {
                Field<FieldValue> generic = instance.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typed = VerifyFilter(selection, typeof(FieldValue));
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = instance.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                FieldNameFilter named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Previous, typeof(FieldValue));
                VerifyScope(typed.Previous, instance);
            }

            static void VerifyScope(IFilter<Field> filter, object instance) {
                var scope = Assert.IsType<InstanceScope>(filter);
                Assert.Same(instance, scope.Instance);
            }
        }

        public class AccessibilityMethod : ObjectExtensionsTest
        {
            readonly object instance = new object();

            [Fact]
            public void InternalReturnsInternalAccessibilityScopeOfGivenInstance() {
                IScope actual = instance.Internal();
                VerifyScope(actual, instance, Accessibility.Internal);
            }

            [Fact]
            public void PrivateReturnsPrivateAccessibilityScopeOfGivenInstance() {
                IScope actual = instance.Private();
                VerifyScope(actual, instance, Accessibility.Private);
            }

            [Fact]
            public void ProtectedReturnsProtectedAccessibilityScopeOfGivenInstance() {
                IScope actual = instance.Protected();
                VerifyScope(actual, instance, Accessibility.Protected);
            }

            [Fact]
            public void PublicReturnsPublicAccessibilityScopeOfGivenInstance() {
                IScope actual = instance.Public();
                VerifyScope(actual, instance, Accessibility.Public);
            }

            static void VerifyScope(IScope actual, object instance, Accessibility accessibility) {
                var accessibilityScope = Assert.IsType<AccessibilityScope>(actual);
                Assert.Equal(accessibility, accessibilityScope.Accessibility);

                var instanceScope = Assert.IsType<InstanceScope>(accessibilityScope.Previous);
                Assert.Same(instance, instanceScope.Instance);
            }
        }
    }
}
