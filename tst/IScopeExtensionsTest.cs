using NSubstitute;
using Xunit;

namespace Inspector
{
    public class IScopeExtensionsTest
    {
        public class AccessibilityMethod : IScopeExtensionsTest
        {
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void InternalReturnsInternalAccessibilityScope() {
                IScope actual = scope.Internal();

                var accessibilityScope = Assert.IsType<AccessibilityScope>(actual);
                Assert.Equal(Accessibility.Internal, accessibilityScope.Accessibility);
            }

            [Fact]
            public void PrivateReturnsPrivateAccessibilityScope() {
                IScope actual = scope.Private();

                var accessibilityScope = Assert.IsType<AccessibilityScope>(actual);
                Assert.Equal(Accessibility.Private, accessibilityScope.Accessibility);
            }

            [Fact]
            public void ProtectedReturnsProtectedAccessibilityScope() {
                IScope actual = scope.Protected();

                var accessibilityScope = Assert.IsType<AccessibilityScope>(actual);
                Assert.Equal(Accessibility.Protected, accessibilityScope.Accessibility);
            }

            [Fact]
            public void PublicReturnsPublicAccessibilityScope() {
                IScope actual = scope.Public();

                var accessibilityScope = Assert.IsType<AccessibilityScope>(actual);
                Assert.Equal(Accessibility.Public, accessibilityScope.Accessibility);
            }
        }
    }
}
