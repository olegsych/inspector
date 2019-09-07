using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests of extension methods that return <see cref="Method"/> and <see cref="Method{T}"/>.
    /// </summary>
    [Collection(nameof(MethodExtensionsTest))]
    public class MethodExtensionsTest: SelectorFixture<Method>
    {
        // Method parameters
        protected readonly Type methodType = typeof(MethodType);
        protected readonly string methodName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Method selected;
        protected IEnumerable<Method> selection;

        public MethodExtensionsTest() {
            selected = new Method(typeof(TestType).GetMethod(nameof(TestType.Method)), instance);
            select.Invoke(Arg.Do<IEnumerable<Method>>(f => selection = f)).Returns(selected);
        }

        protected static void VerifyGenericMethod<T>(Method selected, Method<T> generic) where T : Delegate {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
            Assert.NotNull(generic.Invoke);
        }

        internal static MemberNameFilter<Method, MethodInfo> VerifyFilter(IEnumerable<Method> selection, string methodName) {
            var filter = Assert.IsType<MemberNameFilter<Method, MethodInfo>>(selection);
            Assert.Equal(methodName, filter.MemberName);
            return filter;
        }

        internal static MethodTypeFilter VerifyFilter(IEnumerable<Method> selection, Type expectedMethodType) {
            var filter = Assert.IsType<MethodTypeFilter>(selection);
            Assert.IsType<MethodDelegateFactory>(filter.DelegateFactory);
            Assert.Equal(expectedMethodType, filter.DelegateType);
            return filter;
        }

        protected class TestType
        {
            public void Method(Parameter p1, Parameter p2) { }
        }

        protected class Parameter { }

        protected delegate void MethodType(Parameter p1, Parameter p2);

        public class IScopeExtension: MethodExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            // Arrange
            readonly IEnumerable<Method> methods = Substitute.For<IEnumerable<Method>>();

            public IScopeExtension() =>
                scope.Methods().Returns(methods);

            [Fact]
            public void ReturnsSingleMethodInGivenScope() {
                Assert.Same(selected, scope.Method());
                Assert.Same(methods, selection);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, scope.Method(methodName));
                MemberNameFilter<Method, MethodInfo> filter = VerifyFilter(selection, methodName);
                Assert.Same(methods, filter.Source);
            }

            [Fact]
            public void ReturnsMethodWithGivenType() {
                Assert.Same(selected, scope.Method(methodType));
                MethodTypeFilter filter = VerifyFilter(selection, methodType);
                Assert.Same(methods, filter.Source);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, scope.Method(methodType, methodName));
                MemberNameFilter<Method, MethodInfo> nameFilter = VerifyFilter(selection, methodName);
                MethodTypeFilter typeFilter = VerifyFilter(nameFilter.Source, methodType);
                Assert.Same(methods, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenType() {
                Method<MethodType> generic = scope.Method<MethodType>();
                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typeFilter = VerifyFilter(selection, typeof(MethodType));
                Assert.Same(methods, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = scope.Method<MethodType>(methodName);
                VerifyGenericMethod(selected, generic);
                MemberNameFilter<Method, MethodInfo> nameFilter = VerifyFilter(selection, methodName);
                MethodTypeFilter typeFilter = VerifyFilter(nameFilter.Source, methodType);
                Assert.Same(methods, typeFilter.Source);
            }
        }

        public class ObjectExtension: MethodExtensionsTest
        {
            [Fact]
            public void ReturnsSingleMethodDeclaredByTypeOfGivenInstance() {
                Assert.Same(selected, instance.Method());
                var declared = Assert.IsType<DeclaredMembers<Method, MethodInfo>>(selection);
                Assert.Equal(instance.GetType(), declared.DeclaringType);
                VerifyScope(declared.Source, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenType() {
                Assert.Same(selected, instance.Method(methodType));

                MethodTypeFilter named = VerifyFilter(selection, methodType);
                VerifyScope(named.Source, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, instance.Method(methodName));

                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                VerifyScope(named.Source, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, instance.Method(methodType, methodName));

                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Source, methodType);
                VerifyScope(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericMethodOfGivenType() {
                Method<MethodType> generic = instance.Method<MethodType>();

                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typed = VerifyFilter(selection, typeof(MethodType));
                VerifyScope(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = instance.Method<MethodType>(methodName);

                VerifyGenericMethod(selected, generic);
                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Source, typeof(MethodType));
                VerifyScope(typed.Source, instance);
            }

            static void VerifyScope(IEnumerable<Method> filter, object instance) {
                var scope = Assert.IsType<Members<MethodInfo, Method>>(filter);
                Assert.Same(instance, scope.Instance);
            }
        }

        public class TypeExtension: MethodExtensionsTest
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
                VerifyScope(typed.Source, testType);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, testType.Method(methodName));

                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                VerifyScope(named.Source, testType);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, testType.Method(methodType, methodName));

                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Source, methodType);
                VerifyScope(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericMethodOfGivenType() {
                Method<MethodType> generic = testType.Method<MethodType>();

                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typed = VerifyFilter(selection, typeof(MethodType));
                VerifyScope(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = testType.Method<MethodType>(methodName);

                VerifyGenericMethod(selected, generic);
                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Source, typeof(MethodType));
                VerifyScope(typed.Source, testType);
            }

            static void VerifyScope(IEnumerable<Method> selection, Type expected) {
                var methods = Assert.IsType<Members<MethodInfo, Method>>(selection);
                Assert.Same(expected, methods.Type);
            }
        }
    }
}
