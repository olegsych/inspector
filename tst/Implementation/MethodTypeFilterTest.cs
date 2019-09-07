using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Inspector.Implementation
{
    public class MethodTypeFilterTest
    {
        readonly Filter<Method> sut;

        // Constructor parameters
        readonly IEnumerable<Method> previous = Substitute.For<IEnumerable<Method>>();
        readonly Type delegateType = typeof(Action<P, P>);
        readonly IDelegateFactory<MethodInfo> delegateFactory = Substitute.For<IDelegateFactory<MethodInfo>>();

        public MethodTypeFilterTest() =>
            sut = new MethodTypeFilter(previous, delegateType, delegateFactory);

        public class Constructor: MethodTypeFilterTest
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenPreviousIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MethodTypeFilter(null, delegateType, delegateFactory));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenDelegateTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MethodTypeFilter(previous, null, delegateFactory));
                Assert.Equal("delegateType", thrown.ParamName);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenDelegateFactoryIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MethodTypeFilter(previous, delegateType, null));
                Assert.Equal("delegateFactory", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsInvalid() {
                Type invalid = typeof(InvalidMethodType);
                var thrown = Assert.Throws<ArgumentException>(() => new MethodTypeFilter(previous, invalid, delegateFactory));
                Assert.Equal("delegateType", thrown.ParamName);
                Assert.StartsWith($"{invalid} is not a delegate.", thrown.Message);
            }

            [Fact]
            public void PassesPreviousToBaseConstructor() =>
                Assert.Same(previous, sut.Previous);

            class InvalidMethodType { }
        }

        public class MethodType: MethodTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(delegateType, ((MethodTypeFilter)sut).DelegateType);
        }

        public class DelegateFactory: MethodTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(delegateFactory, ((MethodTypeFilter)sut).DelegateFactory);
        }

        public class GetEnumerator: MethodTypeFilterTest
        {
            [Fact]
            public void ReturnsMethodsWithGivenDelegateType() {
                ConfiguredCall arrange;
                var target = new TestType();
                MethodInfo[] infos = typeof(TestType).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                arrange = delegateFactory.TryCreate(delegateType, target, infos[1], out Delegate @delegate).Returns(true);
                arrange = delegateFactory.TryCreate(delegateType, target, infos[3], out @delegate).Returns(true);

                List<Method> methods = infos.Select(_ => new Method(_, target)).ToList();
                arrange = previous.GetEnumerator().Returns(methods.GetEnumerator());

                Method[] expected = { methods[1], methods[3] };
                Assert.Equal(expected, sut);
            }
        }

        class TestType
        {
            public void M() { }
            public void M(P p1) { }
            public void M(P p1, P p2) { }
            public void M(P p1, P p2, P p3) { }
            public void M(P p1, P p2, P p3, P p4) { }
        }

        class P { }
    }
}
