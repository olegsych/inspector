using System;
using Inspector.Implementation;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class DeclarationExtensionsTest
    {
        public class ObjectExtensions: DeclarationExtensionsTest
        {
            // Method parameters
            readonly object instance = new object();

            [Fact]
            public void ReturnsInstanceMembersDeclaredByGivenType() {
                IMembers actual = instance.DeclaredBy(typeof(TestType));
                VerifyInstanceMembers<TestType>(instance, actual);
            }

            [Fact]
            public void ReturnsInstanceMembersDeclaredByGivenGenericType() {
                IMembers actual = instance.DeclaredBy<TestType>();
                VerifyInstanceMembers<TestType>(instance, actual);
            }

            [Fact]
            public void ReturnsInstanceMembersDeclaredByInstanceType() {
                var instance = new TestType();
                IMembers actual = instance.Declared();
                VerifyInstanceMembers<TestType>(instance, actual);
            }

            static void VerifyInstanceMembers<TDeclaringType>(object instance, IMembers actual) {
                DeclaredMembers declaredMembers = VerifyDeclaredMembers(typeof(TDeclaringType), actual);
                var instanceMembers = Assert.IsType<InstanceMembers>(declaredMembers.Source);
                Assert.Same(instance, instanceMembers.Instance);
            }
        }

        static DeclaredMembers VerifyDeclaredMembers(Type declaringType, IMembers actual) {
            var declaredMembers = Assert.IsType<DeclaredMembers>(actual);
            Assert.Equal(declaringType, declaredMembers.DeclaringType);
            return declaredMembers;
        }

        class TestType { }
    }
}
