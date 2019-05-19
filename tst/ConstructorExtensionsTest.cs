using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    [Collection(nameof(ConstructorExtensionsTest))]
    public class ConstructorExtensionsTest: SelectorFixture<Constructor>
    {
        // Shared test fixture
        class TestType { }
        delegate void TestDelegate();
        readonly object instance = new TestType();
        readonly Type delegateType = typeof(TestDelegate);
        readonly Constructor selected;
        IFilter<Constructor> selection;

        public ConstructorExtensionsTest() {
            selected = new Constructor(typeof(TestType).GetConstructor(new Type[0]), instance);
            select.Invoke(Arg.Do<IFilter<Constructor>>(_ => selection = _)).Returns(selected);
        }

        static ConstructorTypeFilter VerifyFilter(IFilter<Constructor> selection, Type expectedDelegateType) {
            var filter = Assert.IsType<ConstructorTypeFilter>(selection);
            Assert.IsType<ConstructorDelegateFactory>(filter.DelegateFactory);
            Assert.Equal(expectedDelegateType, filter.DelegateType);
            return filter;
        }

        static void VerifyGenericConstructor<T>(Constructor selected, Constructor<T> generic) where T : Delegate {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
            Assert.NotNull(generic.Invoke);
        }

        public class IScopeExtension: ConstructorExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void ReturnsSingleConstructorInGivenScope() {
                Assert.Same(selected, scope.Constructor());
                Assert.Same(scope, selection);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Assert.Same(selected, scope.Constructor(delegateType));
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = scope.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typeFilter = VerifyFilter(selection, typeof(TestDelegate));
                Assert.Same(scope, typeFilter.Previous);
            }
        }

        public class ObjectExtension: ConstructorExtensionsTest
        {
            [Fact]
            public void ReturnsSingleConstructorOfGivenInstance() {
                Assert.Same(selected, instance.Constructor());
                VerifyScope(selection, instance);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Assert.Same(selected, instance.Constructor(delegateType));
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                VerifyScope(filter.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = instance.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typed = VerifyFilter(selection, typeof(TestDelegate));
                VerifyScope(typed.Previous, instance);
            }

            static void VerifyScope(IFilter<Constructor> filter, object instance) {
                var scope = Assert.IsType<InstanceScope>(filter);
                Assert.Same(instance, scope.Instance);
            }
        }

        public class TypeExtension: ConstructorExtensionsTest
        {
            // Method parameters
            readonly Type type;

            public TypeExtension() =>
                type = instance.GetType();

            [Fact]
            public void ReturnsSingleConstructorOfGivenType() {
                Assert.Same(selected, type.Constructor());
                VerifyScope(selection, type);
            }

            static void VerifyScope(IFilter<Constructor> filter, Type type) {
                var scope = Assert.IsType<StaticScope>(filter);
                Assert.Same(type, scope.Type);
            }
        }
    }
}
