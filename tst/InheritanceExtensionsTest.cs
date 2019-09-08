using System;
using Inspector.Implementation;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class InheritanceExtensionsTest
    {
        public class IMembersExtensions: InheritanceExtensionsTest
        {
            // Method parameters
            readonly IMembers members = Substitute.For<IMembers>();

            [Fact]
            public void InheritedFromReturnsMembersInheritedFromGivenType() {
                IMembers actual = members.InheritedFrom(typeof(TestType));
                VerifyInheritedMembers<TestType>(members, actual);
            }

            [Fact]
            public void GenericInheritedFromReturnsMembersInheritedFromGivenType() {
                IMembers actual = members.InheritedFrom<TestType>();
                VerifyInheritedMembers<TestType>(members, actual);
            }

            static void VerifyInheritedMembers<TAncestorType>(IMembers expected, IMembers actual) {
                InheritedMembers inheritedMembers = VerifyInheritedMembers(typeof(TAncestorType), actual);
                Assert.Same(expected, inheritedMembers.Source);
            }
        }

        public class ObjectExtensions: InheritanceExtensionsTest
        {
            // Method parameters
            readonly object instance = new object();

            [Fact]
            public void InheritedReturnsInstanceMembersInheritedFromBaseType() {
                var instance = new TestType();
                IMembers actual = instance.Inherited();
                VerifyInheritedMembers<BaseType>(instance, actual);
            }

            [Fact]
            public void InheritedFromReturnsInstanceMembersInheritedFromGivenType() {
                IMembers actual = instance.InheritedFrom(typeof(TestType));
                VerifyInheritedMembers<TestType>(instance, actual);
            }

            [Fact]
            public void GenericInheritedFromReturnsInstanceMembersInheritedFromGivenType() {
                IMembers actual = instance.InheritedFrom<TestType>();
                VerifyInheritedMembers<TestType>(instance, actual);
            }

            static void VerifyInheritedMembers<TAncestorType>(object instance, IMembers actual) {
                InheritedMembers inheritedMembers = VerifyInheritedMembers(typeof(TAncestorType), actual);
                var instanceMembers = Assert.IsType<InstanceMembers>(inheritedMembers.Source);
                Assert.Same(instance, instanceMembers.Instance);
            }
        }

        public class TypeExtensions: InheritanceExtensionsTest
        {
            // Method parameters
            readonly Type type = Type();

            [Fact]
            public void InheritedReturnsStaticMembersInheritedFromBaseType() {
                IMembers actual = typeof(TestType).Inherited();
                VerifyInheritedMembers<BaseType>(typeof(TestType), actual);
            }

            [Fact]
            public void InheritedFromReturnsStaticMembersInheritedFromGivenType() {
                IMembers actual = type.InheritedFrom(typeof(TestType));
                VerifyInheritedMembers<TestType>(type, actual);
            }

            [Fact]
            public void GenericInheritedFromReturnsStaticMembersInheritedFromGivenType() {
                IMembers actual = type.InheritedFrom<TestType>();
                VerifyInheritedMembers<TestType>(type, actual);
            }

            static void VerifyInheritedMembers<TDeclaringType>(Type staticType, IMembers actual) {
                InheritedMembers inheritedMembers = VerifyInheritedMembers(typeof(TDeclaringType), actual);
                var staticMembers = Assert.IsType<StaticMembers>(inheritedMembers.Source);
                Assert.Equal(staticType, staticMembers.Type);
            }
        }

        static InheritedMembers VerifyInheritedMembers(Type ancestorType, IMembers actual) {
            var inheritedMembers = Assert.IsType<InheritedMembers>(actual);
            Assert.Equal(ancestorType, inheritedMembers.AncestorType);
            return inheritedMembers;
        }

        class BaseType { }
        class TestType : BaseType { }
    }
}
