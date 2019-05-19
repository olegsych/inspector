using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class MethodTypeFilterTest
    {
        readonly IFilter<Method> sut;

        // Constructor parameters
        readonly IFilter<Method> previous = Substitute.For<IFilter<Method>>();
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

        public class Previous: MethodTypeFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Method>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get: MethodTypeFilterTest
        {
            [Fact]
            public void ReturnsMethodsWithGivenDelegateType() {
                // Arrange
                MethodInfo[] infos = typeof(TestType).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                var target = new TestType();
                delegateFactory.TryCreate(delegateType, target, infos[1], out Delegate @delegate).Returns(true);
                delegateFactory.TryCreate(delegateType, target, infos[3], out @delegate).Returns(true);

                Method[] methods = infos.Select(_ => new Method(_, target)).ToArray();
                previous.Get().Returns(methods);

                // Act
                IEnumerable<Method> actual = sut.Get();

                // Assert
                Method[] expected = { methods[1], methods[3] };
                Assert.Equal(expected, actual);
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
