using System.Collections.Generic;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class IMembersExtensionsTest
    {
        readonly IMembers members = Substitute.For<IMembers>();

        public class ConstructorTest: ConstructorExtensionsTest
        {
            readonly IMembers members = Substitute.For<IMembers>();

            // Test fixture
            readonly IEnumerable<Constructor> constructors = Substitute.For<IEnumerable<Constructor>>();

            public ConstructorTest() =>
                members.Constructors().Returns(constructors);

            [Fact]
            public void ReturnsSingleConstructor() {
                Constructor actual = members.Constructor();

                Assert.Same(selected, actual);
                Assert.Same(constructors, selection);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Constructor actual = members.Constructor(delegateType);

                Assert.Same(selected, actual);
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                Assert.Same(constructors, filter.Source);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = members.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typeFilter = VerifyFilter(selection, typeof(TestDelegate));
                Assert.Same(constructors, typeFilter.Source);
            }
        }

        [Fact]
        public void DeclaredByReturnsMembersDeclaredByGivenType() {
            IMembers actual = members.DeclaredBy(typeof(TestType));

            VerifyDeclaredMembers<TestType>(members, actual);
        }

        [Fact]
        public void DeclaredByGenericReturnsMembersDeclaredByGivenType() {
            IMembers actual = members.DeclaredBy<TestType>();

            VerifyDeclaredMembers<TestType>(members, actual);
        }

        [Fact]
        public void InternalReturnsInternalMembers() {
            IMembers actual = members.Internal();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Internal, accessibleMembers.Accessibility);
        }

        [Fact]
        public void InheritedFromReturnsMembersInheritedFromGivenType() {
            IMembers actual = members.InheritedFrom(typeof(TestType));

            VerifyInheritedMembers<TestType>(members, actual);
        }

        [Fact]
        public void InheritedFromGenericReturnsMembersInheritedFromGivenType() {
            IMembers actual = members.InheritedFrom<TestType>();

            VerifyInheritedMembers<TestType>(members, actual);
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

        static void VerifyDeclaredMembers<TDeclaringType>(IMembers source, IMembers actual) {
            var declaredMembers = Assert.IsType<DeclaredMembers>(actual);
            Assert.Equal(typeof(TDeclaringType), declaredMembers.DeclaringType);
            Assert.Same(source, declaredMembers.Source);
        }

        static void VerifyInheritedMembers<TAncestorType>(IMembers source, IMembers actual) {
            var inheritedMembers = Assert.IsType<InheritedMembers>(actual);
            Assert.Equal(typeof(TAncestorType), inheritedMembers.AncestorType);
            Assert.Same(source, inheritedMembers.Source);
        }

        class TestType {}
    }
}
