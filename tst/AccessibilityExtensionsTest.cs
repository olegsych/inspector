using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class AccessibilityExtensionsTest
    {
        public class IScopeExtension: AccessibilityExtensionsTest
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

        public class ObjectExtension: AccessibilityExtensionsTest
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

        public class TypeExtension: AccessibilityExtensionsTest
        {
            readonly Type type = typeof(TestClass);

            [Fact]
            public void InternalReturnsInternalAccessibilityScopeOfGivenType() {
                IScope actual = type.Internal();
                VerifyScope(actual, type, Accessibility.Internal);
            }

            [Fact]
            public void PrivateReturnsPrivateAccessibilityScopeOfGivenType() {
                IScope actual = type.Private();
                VerifyScope(actual, type, Accessibility.Private);
            }

            [Fact]
            public void ProtectedReturnsProtectedAccessibilityScopeOfGivenType() {
                IScope actual = type.Protected();
                VerifyScope(actual, type, Accessibility.Protected);
            }

            [Fact]
            public void PublicReturnsPublicAccessibilityScopeOfGivenType() {
                IScope actual = type.Public();
                VerifyScope(actual, type, Accessibility.Public);
            }

            static void VerifyScope(IScope actual, Type type, Accessibility accessibility) {
                var accessibilityScope = Assert.IsType<AccessibilityScope>(actual);
                Assert.Equal(accessibility, accessibilityScope.Accessibility);

                var instanceScope = Assert.IsType<StaticScope>(accessibilityScope.Previous);
                Assert.Same(type, instanceScope.Type);
            }

            class TestClass { }
        }
    }
}
