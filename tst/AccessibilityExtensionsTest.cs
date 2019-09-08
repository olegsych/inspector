using System;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class AccessibilityExtensionsTest
    {
        public class IMembersExtension: AccessibilityExtensionsTest
        {
            readonly IMembers members = Substitute.For<IMembers>();

            [Fact]
            public void InternalReturnsInternalMembers() {
                IMembers actual = members.Internal();

                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(Accessibility.Internal, accessibleMembers.Accessibility);
            }

            [Fact]
            public void PrivateReturnsPrivateMembers() {
                IMembers actual = members.Private();

                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(Accessibility.Private, accessibleMembers.Accessibility);
            }

            [Fact]
            public void ProtectedReturnsProtectedMembers() {
                IMembers actual = members.Protected();

                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(Accessibility.Protected, accessibleMembers.Accessibility);
            }

            [Fact]
            public void PublicReturnsPublicMembers() {
                IMembers actual = members.Public();

                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(Accessibility.Public, accessibleMembers.Accessibility);
            }
        }

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

        public class TypeExtension: AccessibilityExtensionsTest
        {
            readonly Type type = typeof(TestClass);

            [Fact]
            public void InternalReturnsInternalMembersOfGivenType() {
                IMembers actual = type.Internal();
                VerifyMembers(actual, type, Accessibility.Internal);
            }

            [Fact]
            public void PrivateReturnsPrivateMembersOfGivenType() {
                IMembers actual = type.Private();
                VerifyMembers(actual, type, Accessibility.Private);
            }

            [Fact]
            public void ProtectedReturnsProtectedMembersOfGivenType() {
                IMembers actual = type.Protected();
                VerifyMembers(actual, type, Accessibility.Protected);
            }

            [Fact]
            public void PublicReturnsPublicMembersOfGivenType() {
                IMembers actual = type.Public();
                VerifyMembers(actual, type, Accessibility.Public);
            }

            static void VerifyMembers(IMembers actual, Type type, Accessibility accessibility) {
                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(accessibility, accessibleMembers.Accessibility);

                var staticMembers = Assert.IsType<StaticMembers>(accessibleMembers.Source);
                Assert.Same(type, staticMembers.Type);
            }

            class TestClass { }
        }
    }
}
