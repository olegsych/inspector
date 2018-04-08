using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests of extension methods that return <see cref="Method"/> and <see cref="Method{T}"/>.
    /// </summary>
    [Collection(nameof(MethodExtensionsTest))]
    public class MethodExtensionsTest : SelectorFixture<Method>
    {
        // Method parameters
        protected readonly Type methodType = typeof(MethodType);
        protected readonly string methodName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Method selected;
        protected IFilter<Method> selection;

        public MethodExtensionsTest() {
            selected = new Method(typeof(TestType).GetMethod(nameof(TestType.Method)), instance);
            select.Invoke(Arg.Do<IFilter<Method>>(f => selection = f)).Returns(selected);
        }

        protected static void VerifyGenericMethod<T>(Method selected, Method<T> generic) {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        internal static MethodNameFilter VerifyFilter(IFilter<Method> selection, string methodName) {
            var filter = Assert.IsType<MethodNameFilter>(selection);
            Assert.Equal(methodName, filter.MethodName);
            return filter;
        }

        internal static MethodTypeFilter VerifyFilter(IFilter<Method> selection, Type expectedMethodType) {
            var filter = Assert.IsType<MethodTypeFilter>(selection);
            Assert.Equal(expectedMethodType, filter.MethodType);
            return filter;
        }

        protected class TestType
        {
            public void Method(Parameter p1, Parameter p2) { }
        }

        protected class Parameter { }

        protected delegate void MethodType(Parameter p1, Parameter p2);

        public class IScopeExtension : MethodExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void ReturnsSingleMethodInGivenScope() {
                Assert.Same(selected, scope.Method());

                Assert.Same(scope, selection);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, scope.Method(methodName));

                MethodNameFilter filter = VerifyFilter(selection, methodName);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsMethodWithGivenType() {
                Assert.Same(selected, scope.Method(methodType));

                MethodTypeFilter filter = VerifyFilter(selection, methodType);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, scope.Method(methodType, methodName));

                MethodNameFilter nameFilter = VerifyFilter(selection, methodName);
                MethodTypeFilter typeFilter = VerifyFilter(nameFilter.Previous, methodType);
                Assert.Same(scope, typeFilter.Previous);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenType() {
                Method<MethodType> generic = scope.Method<MethodType>();

                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typeFilter = VerifyFilter(selection, typeof(MethodType));
                Assert.Same(scope, typeFilter.Previous);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = scope.Method<MethodType>(methodName);

                VerifyGenericMethod(selected, generic);
                MethodNameFilter nameFilter = VerifyFilter(selection, methodName);
                MethodTypeFilter typeFilter = VerifyFilter(nameFilter.Previous, methodType);
                Assert.Same(scope, typeFilter.Previous);
            }
        }

        public class ObjectExtension : MethodExtensionsTest
        {
            [Fact]
            public void ReturnsSingleMethodInGivenType() {
                Assert.Same(selected, instance.Method());

                VerifyScope(selection, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenType() {
                Assert.Same(selected, instance.Method(methodType));

                MethodTypeFilter named = VerifyFilter(selection, methodType);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, instance.Method(methodName));

                MethodNameFilter named = VerifyFilter(selection, methodName);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, instance.Method(methodType, methodName));

                MethodNameFilter named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Previous, methodType);
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericMethodOfGivenType() {
                Method<MethodType> generic = instance.Method<MethodType>();

                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typed = VerifyFilter(selection, typeof(MethodType));
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = instance.Method<MethodType>(methodName);

                VerifyGenericMethod(selected, generic);
                MethodNameFilter named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Previous, typeof(MethodType));
                VerifyScope(typed.Previous, instance);
            }

            static void VerifyScope(IFilter<Method> filter, object instance) {
                var scope = Assert.IsType<InstanceScope>(filter);
                Assert.Same(instance, scope.Instance);
            }
        }

        public class TypeExtension : MethodExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSingleMethodInGivenType() {
                Assert.Same(selected, testType.Method());

                VerifyScope(selection, testType);
            }

            [Fact]
            public void ReturnsMethodWithGivenType() {
                Assert.Same(selected, testType.Method(methodType));

                MethodTypeFilter typed = VerifyFilter(selection, methodType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, testType.Method(methodName));

                MethodNameFilter named = VerifyFilter(selection, methodName);
                VerifyScope(named.Previous, testType);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, testType.Method(methodType, methodName));

                MethodNameFilter named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Previous, methodType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsGenericMethodOfGivenType() {
                Method<MethodType> generic = testType.Method<MethodType>();

                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typed = VerifyFilter(selection, typeof(MethodType));
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = testType.Method<MethodType>(methodName);

                VerifyGenericMethod(selected, generic);
                MethodNameFilter named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Previous, typeof(MethodType));
                VerifyScope(typed.Previous, testType);
            }

            static void VerifyScope(IFilter<Method> selection, Type expected) {
                var scope = Assert.IsType<StaticScope>(selection);
                Assert.Same(expected, scope.Type);
            }
        }
    }
}
