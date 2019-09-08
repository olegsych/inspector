using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest
    {
        readonly object instance = new object();

        public class AccessibilityMethod: ObjectExtensionsTest
        {
            [Fact]
            public void InternalReturnsInternalMembersOfGivenInstance() {
                IMembers actual = instance.Internal();
                VerifyAccessibleMembers(instance, Accessibility.Internal, actual);
            }

            [Fact]
            public void PrivateReturnsPrivateMembersOfGivenInstance() {
                IMembers actual = instance.Private();
                VerifyAccessibleMembers(instance, Accessibility.Private, actual);
            }

            [Fact]
            public void ProtectedReturnsProtectedMembersOfGivenInstance() {
                IMembers actual = instance.Protected();
                VerifyAccessibleMembers(instance, Accessibility.Protected, actual);
            }

            [Fact]
            public void PublicReturnsPublicMembersOfGivenInstance() {
                IMembers actual = instance.Public();
                VerifyAccessibleMembers(instance, Accessibility.Public, actual);
            }

            static void VerifyAccessibleMembers(object instance, Accessibility accessibility, IMembers actual)
            {
                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(accessibility, accessibleMembers.Accessibility);
                VerifyInstanceMembers(instance, accessibleMembers.Source);
            }
        }

        public class ConstructorMethod: ConstructorExtensionsTest
        {
            readonly object instance = new object();

            [Fact]
            public void ReturnsSingleConstructorDeclaredByTypeOfGivenInstance() {
                Assert.Same(selected, instance.Constructor());
                var declared = Assert.IsType<DeclarationFilter<Constructor, ConstructorInfo>>(selection);
                Assert.Equal(instance.GetType(), declared.DeclaringType);
                VerifyInstanceConstructors(declared.Source, instance);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Assert.Same(selected, instance.Constructor(delegateType));
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                VerifyInstanceConstructors(filter.Source, instance);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = instance.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typed = VerifyFilter(selection, typeof(TestDelegate));
                VerifyInstanceConstructors(typed.Source, instance);
            }

            static void VerifyInstanceConstructors(IEnumerable<Constructor> filter, object instance) {
                var members = Assert.IsType<Members<ConstructorInfo, Constructor>>(filter);
                Assert.Same(instance, members.Instance);
            }
        }

        public class DeclaredMethod: ObjectExtensionsTest
        {
            [Fact]
            public void ReturnsInstanceMembersDeclaredByGivenType() {
                IMembers actual = instance.DeclaredBy(typeof(TestType));
                VerifyDeclaredMembers<TestType>(instance, actual);
            }

            [Fact]
            public void ReturnsInstanceMembersDeclaredByGivenGenericType() {
                IMembers actual = instance.DeclaredBy<TestType>();
                VerifyDeclaredMembers<TestType>(instance, actual);
            }

            [Fact]
            public void ReturnsInstanceMembersDeclaredByInstanceType() {
                var instance = new TestType();
                IMembers actual = instance.Declared();
                VerifyDeclaredMembers<TestType>(instance, actual);
            }

            static void VerifyDeclaredMembers<TDeclaringType>(object instance, IMembers actual) {
                var declaredMembers = Assert.IsType<DeclaredMembers>(actual);
                Assert.Equal(typeof(TDeclaringType), declaredMembers.DeclaringType);
                VerifyInstanceMembers(instance, declaredMembers.Source);
            }

            class TestType { }
        }

        internal static void VerifyInstanceMembers(object instance, IMembers actual) {
            var instanceMembers = Assert.IsType<InstanceMembers>(actual);
            Assert.Same(instance, instanceMembers.Instance);
        }
    }
}
