using System;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class AccessibilityExtensionsTest
    {
        public class ObjectExtension: AccessibilityExtensionsTest
        {
            readonly object instance = new object();

            [Fact]
            public void InternalReturnsInternalMembersOfGivenInstance() {
                IMembers actual = instance.Internal();
                VerifyMembers(actual, instance, Accessibility.Internal);
            }

            [Fact]
            public void PrivateReturnsPrivateMembersOfGivenInstance() {
                IMembers actual = instance.Private();
                VerifyMembers(actual, instance, Accessibility.Private);
            }

            [Fact]
            public void ProtectedReturnsProtectedMembersOfGivenInstance() {
                IMembers actual = instance.Protected();
                VerifyMembers(actual, instance, Accessibility.Protected);
            }

            [Fact]
            public void PublicReturnsPublicMembersOfGivenInstance() {
                IMembers actual = instance.Public();
                VerifyMembers(actual, instance, Accessibility.Public);
            }

            static void VerifyMembers(IMembers actual, object instance, Accessibility accessibility) {
                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(accessibility, accessibleMembers.Accessibility);

                var instanceMembers = Assert.IsType<InstanceMembers>(accessibleMembers.Source);
                Assert.Same(instance, instanceMembers.Instance);
            }
        }
    }
}
