using System;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class DeclarationExtensionsTest
    {
        public class IScopeExtensions: DeclarationExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void ReturnsScopeLimitedToDeclarationScopeOfGivenType() {
                IScope actual = scope.DeclaredBy(typeof(TestType));
                VerifyDeclarationScope<TestType>(scope, actual);
            }

            [Fact]
            public void ReturnsScopeLimitedToDeclarationScopeOfGivenTypeParameter() {
                IScope actual = scope.DeclaredBy<TestType>();
                VerifyDeclarationScope<TestType>(scope, actual);
            }

            static void VerifyDeclarationScope<TDeclaringType>(IScope expected, IScope actual) {
                DeclarationScope declarationScope = VerifyDeclarationScope(typeof(TDeclaringType), actual);
                Assert.Same(expected, declarationScope.Previous);
            }
        }

        public class ObjectExtensions: DeclarationExtensionsTest
        {
            // Method parameters
            readonly object instance = new object();

            [Fact]
            public void ReturnsInstanceScopeLimitedToDeclarationScopeOfGivenType() {
                IScope actual = instance.DeclaredBy(typeof(TestType));
                VerifyInstanceScope<TestType>(instance, actual);
            }

            [Fact]
            public void ReturnsInstanceScopeLimitedToDeclarationScopeOfGivenTypeParameter() {
                IScope actual = instance.DeclaredBy<TestType>();
                VerifyInstanceScope<TestType>(instance, actual);
            }

            [Fact]
            public void ReturnsInstanceScopeLimitedToDeclarationScopeOfInstanceType() {
                var instance = new TestType();
                IScope actual = instance.Declared();
                VerifyInstanceScope<TestType>(instance, actual);
            }

            static void VerifyInstanceScope<TDeclaringType>(object instance, IScope actual) {
                DeclarationScope declarationScope = VerifyDeclarationScope(typeof(TDeclaringType), actual);
                var instanceScope = Assert.IsType<InstanceScope>(declarationScope.Previous);
                Assert.Same(instance, instanceScope.Instance);
            }
        }

        public class TypeExtensions: DeclarationExtensionsTest
        {
            // Method parameters
            readonly Type type = Type();

            [Fact]
            public void ReturnsStaticScopeLimitedToGivenDeclaringType() {
                IScope actual = type.DeclaredBy(typeof(TestType));
                VerifyStaticScope<TestType>(type, actual);
            }

            [Fact]
            public void ReturnsStaticScopeLimitedToGivenDeclaringTypeParameter() {
                IScope actual = type.DeclaredBy<TestType>();
                VerifyStaticScope<TestType>(type, actual);
            }

            [Fact]
            public void ReturnsStaticScopeLimitedToTypeItself() {
                IScope actual = typeof(TestType).Declared();
                VerifyStaticScope<TestType>(typeof(TestType), actual);
            }

            static void VerifyStaticScope<TDeclaringType>(Type staticType, IScope actual) {
                DeclarationScope declarationScope = VerifyDeclarationScope(typeof(TDeclaringType), actual);
                var staticScope = Assert.IsType<StaticScope>(declarationScope.Previous);
                Assert.Equal(staticType, staticScope.Type);
            }
        }

        static DeclarationScope VerifyDeclarationScope(Type declaringType, IScope actual) {
            var declarationScope = Assert.IsType<DeclarationScope>(actual);
            Assert.Equal(declaringType, declarationScope.DeclaringType);
            return declarationScope;
        }

        class TestType { }
    }
}
