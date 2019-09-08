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

        public class TypeExtensions: DeclarationExtensionsTest
        {
            // Method parameters
            readonly Type type = Type();

            [Fact]
            public void ReturnsStaticMembersDeclaredByGivenType() {
                IMembers actual = type.DeclaredBy(typeof(TestType));
                VerifyStaticMembers<TestType>(type, actual);
            }

            [Fact]
            public void ReturnsStaticMembersDeclaredByGivenGenericType() {
                IMembers actual = type.DeclaredBy<TestType>();
                VerifyStaticMembers<TestType>(type, actual);
            }

            [Fact]
            public void ReturnsStaticMembersDeclaredByTypeItself() {
                IMembers actual = typeof(TestType).Declared();
                VerifyStaticMembers<TestType>(typeof(TestType), actual);
            }

            static void VerifyStaticMembers<TDeclaringType>(Type staticType, IMembers actual) {
                DeclaredMembers declaredMembers = VerifyDeclaredMembers(typeof(TDeclaringType), actual);
                var staticMembers = Assert.IsType<StaticMembers>(declaredMembers.Source);
                Assert.Equal(staticType, staticMembers.Type);
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
