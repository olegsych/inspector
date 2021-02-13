using System;
using System.Collections.Generic;
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
        protected IEnumerable<Constructor>? selection;

        public ConstructorExtensionsTest() {
            selected = new Constructor(typeof(TestType).GetConstructor(new Type[0])!, instance);
            object arrange = select.Invoke(Arg.Do<IEnumerable<Constructor>>(_ => selection = _)).Returns(selected);
        }

        internal static ConstructorTypeFilter VerifyFilter(IEnumerable<Constructor> selection, Type expectedDelegateType) {
            var filter = (ConstructorTypeFilter)selection;
            var assert = (ConstructorDelegateFactory)filter.DelegateFactory;
            Assert.Equal(expectedDelegateType, filter.DelegateType);
            return filter;
        }

        internal static void VerifyGenericConstructor<T>(Constructor selected, Constructor<T> generic) where T : Delegate {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
            Assert.NotNull(generic.Invoke);
        }
    }
}
