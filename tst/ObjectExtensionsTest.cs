using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest
    {
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
