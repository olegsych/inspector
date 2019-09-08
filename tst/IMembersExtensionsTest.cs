using System.Collections.Generic;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class IMembersExtensionsTest
    {
        readonly IMembers members = Substitute.For<IMembers>();

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
            DeclaredMembers declaredMembers = Assert.IsType<DeclaredMembers>(actual);
            Assert.Equal(typeof(TDeclaringType), declaredMembers.DeclaringType);
            Assert.Same(source, declaredMembers.Source);
        }

        class TestType {}
    }
}
