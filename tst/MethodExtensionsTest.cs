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
        protected IEnumerable<Method>? selection;

        public MethodExtensionsTest() {
            selected = new Method(typeof(TestType).GetMethod(nameof(TestType.Method))!, instance);
            object arrange = select.Invoke(Arg.Do<IEnumerable<Method>>(f => selection = f)).Returns(selected);
        }

        protected static void VerifyGenericMethod<T>(Method selected, Method<T> generic) where T : Delegate {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
            Assert.NotNull(generic.Invoke);
        }

        internal static MemberNameFilter<Method, MethodInfo> VerifyFilter(IEnumerable<Method>? selection, string methodName) {
            Assert.NotNull(selection);
            var filter = (MemberNameFilter<Method, MethodInfo>)selection!;
            Assert.Equal(methodName, filter.MemberName);
            return filter;
        }

        internal static MethodTypeFilter VerifyFilter(IEnumerable<Method>? selection, Type expectedMethodType) {
            Assert.NotNull(selection);
            var filter = (MethodTypeFilter)selection!;
            object assert = (MethodDelegateFactory)filter.DelegateFactory;
            Assert.Equal(expectedMethodType, filter.DelegateType);
            return filter;
        }

        protected class TestType
        {
            public void Method(Parameter p1, Parameter p2) { }
        }

        protected class Parameter { }

        protected delegate void MethodType(Parameter p1, Parameter p2);
    }
}
