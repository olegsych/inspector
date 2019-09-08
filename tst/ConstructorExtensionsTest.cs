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
        protected delegate void TestDelegate();
        readonly object instance = new TestType();
        protected readonly Type delegateType = typeof(TestDelegate);
        protected readonly Constructor selected;
        protected IEnumerable<Constructor> selection;

        public ConstructorExtensionsTest() {
            selected = new Constructor(typeof(TestType).GetConstructor(new Type[0]), instance);
            select.Invoke(Arg.Do<IEnumerable<Constructor>>(_ => selection = _)).Returns(selected);
        }

        internal static ConstructorTypeFilter VerifyFilter(IEnumerable<Constructor> selection, Type expectedDelegateType) {
            var filter = Assert.IsType<ConstructorTypeFilter>(selection);
            Assert.IsType<ConstructorDelegateFactory>(filter.DelegateFactory);
            Assert.Equal(expectedDelegateType, filter.DelegateType);
            return filter;
        }

        internal static void VerifyGenericConstructor<T>(Constructor selected, Constructor<T> generic) where T : Delegate {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
            Assert.NotNull(generic.Invoke);
        }

        public class ObjectExtension: ConstructorExtensionsTest
        {
            [Fact]
            public void ReturnsSingleConstructorDeclaredByTypeOfGivenInstance() {
                Assert.Same(selected, instance.Constructor());
                var declared = Assert.IsType<DeclarationFilter<Constructor, ConstructorInfo>>(selection);
                Assert.Equal(instance.GetType(), declared.DeclaringType);
                VerifyMembers(declared.Source, instance);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Assert.Same(selected, instance.Constructor(delegateType));
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                VerifyMembers(filter.Source, instance);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = instance.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typed = VerifyFilter(selection, typeof(TestDelegate));
                VerifyMembers(typed.Source, instance);
            }

            static void VerifyMembers(IEnumerable<Constructor> filter, object instance) {
                var members = Assert.IsType<Members<ConstructorInfo, Constructor>>(filter);
                Assert.Same(instance, members.Instance);
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
                var members = Assert.IsType<Members<ConstructorInfo, Constructor>>(selection);
                Assert.Same(type, members.Type);
            }
        }
    }
}
