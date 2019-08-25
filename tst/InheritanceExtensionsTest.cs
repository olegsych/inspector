using System;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class InheritanceExtensionsTest
    {
        public class IScopeExtensions: InheritanceExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void InheritedFromReturnsScopeLimitedToInheritanceScopeOfGivenAncestorType() {
                IScope actual = scope.InheritedFrom(typeof(TestType));
                VerifyInheritanceScope<TestType>(scope, actual);
            }

            [Fact]
            public void InheritedFromReturnsScopeLimitedToInheritanceScopeOfGivenAncestorTypeParameter() {
                IScope actual = scope.InheritedFrom<TestType>();
                VerifyInheritanceScope<TestType>(scope, actual);
            }

            static void VerifyInheritanceScope<TAncestorType>(IScope expected, IScope actual) {
                InheritanceScope inheritanceScope = VerifyInheritanceScope(typeof(TAncestorType), actual);
                Assert.Same(expected, inheritanceScope.Previous);
            }
        }

        public class ObjectExtensions: InheritanceExtensionsTest
        {
            // Method parameters
            readonly object instance = new object();

            [Fact]
            public void InheritedReturnsInstanceScopeLimitedToInheritanceScopeOfBaseType() {
                var instance = new TestType();
                IScope actual = instance.Inherited();
                VerifyInstanceScope<BaseType>(instance, actual);
            }

            [Fact]
            public void InheritedFromReturnsInstanceScopeLimitedToInheritanceScopeOfGivenAncestorType() {
                IScope actual = instance.InheritedFrom(typeof(TestType));
                VerifyInstanceScope<TestType>(instance, actual);
            }

            [Fact]
            public void InheritedFromReturnsInstanceScopeLimitedToInheritanceScopeOfGivenAncestorTypeParameter() {
                IScope actual = instance.InheritedFrom<TestType>();
                VerifyInstanceScope<TestType>(instance, actual);
            }

            static void VerifyInstanceScope<TAncestorType>(object instance, IScope actual) {
                InheritanceScope inheritanceScope = VerifyInheritanceScope(typeof(TAncestorType), actual);
                var instanceScope = Assert.IsType<InstanceScope>(inheritanceScope.Previous);
                Assert.Same(instance, instanceScope.Instance);
            }
        }

        public class TypeExtensions: InheritanceExtensionsTest
        {
            // Method parameters
            readonly Type type = Type();

            [Fact]
            public void InheritedReturnsStaticScopeLimitedToInheritanceScopeOfBaseType() {
                IScope actual = typeof(TestType).Inherited();
                VerifyStaticScope<BaseType>(typeof(TestType), actual);
            }

            [Fact]
            public void InheritedFromReturnsStaticScopeLimitedToInheritanceScopeOfGivenAcestorType() {
                IScope actual = type.InheritedFrom(typeof(TestType));
                VerifyStaticScope<TestType>(type, actual);
            }

            [Fact]
            public void InheritedFromReturnsStaticScopeLimitedToInheritanceScopeOfGivenAncestorTypeParameter() {
                IScope actual = type.InheritedFrom<TestType>();
                VerifyStaticScope<TestType>(type, actual);
            }

            static void VerifyStaticScope<TDeclaringType>(Type staticType, IScope actual) {
                InheritanceScope inheritanceScope = VerifyInheritanceScope(typeof(TDeclaringType), actual);
                var staticScope = Assert.IsType<StaticScope>(inheritanceScope.Previous);
                Assert.Equal(staticType, staticScope.Type);
            }
        }

        static InheritanceScope VerifyInheritanceScope(Type ancestorType, IScope actual) {
            var inheritanceScope = Assert.IsType<InheritanceScope>(actual);
            Assert.Equal(ancestorType, inheritanceScope.AncestorType);
            return inheritanceScope;
        }

        class BaseType { }
        class TestType : BaseType { }
    }
}
