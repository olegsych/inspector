using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
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
        IEnumerable<Constructor> selection;

        public ConstructorExtensionsTest() {
            selected = new Constructor(typeof(TestType).GetConstructor(new Type[0]), instance);
            select.Invoke(Arg.Do<IEnumerable<Constructor>>(_ => selection = _)).Returns(selected);
        }

        static ConstructorTypeFilter VerifyFilter(IEnumerable<Constructor> selection, Type expectedDelegateType) {
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

            // Test fixture
            readonly IEnumerable<Constructor> constructors = Substitute.For<IEnumerable<Constructor>>();

            public IScopeExtension() =>
                scope.Constructors().Returns(constructors);

            [Fact]
            public void ReturnsSingleConstructorInGivenScope() {
                Assert.Same(selected, scope.Constructor());
                Assert.Same(constructors, selection);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Assert.Same(selected, scope.Constructor(delegateType));
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                Assert.Same(constructors, filter.Source);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = scope.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typeFilter = VerifyFilter(selection, typeof(TestDelegate));
                Assert.Same(constructors, typeFilter.Source);
            }
        }

        public class ObjectExtension: ConstructorExtensionsTest
        {
            [Fact]
            public void ReturnsSingleConstructorDeclaredByTypeOfGivenInstance() {
                Assert.Same(selected, instance.Constructor());
                var declared = Assert.IsType<DeclaredMembers<Constructor, ConstructorInfo>>(selection);
                Assert.Equal(instance.GetType(), declared.DeclaringType);
                VerifyScope(declared.Source, instance);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Assert.Same(selected, instance.Constructor(delegateType));
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                VerifyScope(filter.Source, instance);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = instance.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typed = VerifyFilter(selection, typeof(TestDelegate));
                VerifyScope(typed.Source, instance);
            }

            static void VerifyScope(IEnumerable<Constructor> filter, object instance) {
                var scope = Assert.IsType<Members<ConstructorInfo, Constructor>>(filter);
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

            static void VerifyScope(IEnumerable<Constructor> filter, Type type) {
                var members = Assert.IsType<Members<ConstructorInfo, Constructor>>(filter);
                Assert.Same(type, members.Type);
            }
        }
    }
}
