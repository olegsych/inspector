using System;
using System.Collections.Generic;
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

        public MethodTypeFilterTest() =>
            sut = new MethodTypeFilter(previous, delegateType);

        public class Constructor : MethodTypeFilterTest
        {
            [Fact]
            public void ThrowsArgumentNullExceptionIfMethodsArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MethodTypeFilter(null, delegateType));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionIfMethodTypeArgumentIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MethodTypeFilter(previous, null));
                Assert.Equal("methodType", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodTypeIsNotDelegate() {
                Type invalid = typeof(InvalidMethodType);
                var thrown = Assert.Throws<ArgumentException>(() => new MethodTypeFilter(previous, invalid));
                Assert.Equal("methodType", thrown.ParamName);
                Assert.StartsWith($"{invalid} is not a delegate.", thrown.Message);
            }

            class InvalidMethodType { }
        }

        public class MethodType : MethodTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(delegateType, ((MethodTypeFilter)sut).MethodType);
        }

        public class Previous : MethodTypeFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Method>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get : MethodTypeFilterTest
        {
            [Fact]
            public void ReturnsMethodsWithGivenMethodType() {
                // Arrange
                MethodInfo methodInfo = typeof(TestType).GetMethod(nameof(TestType.Method2));

                var expected = new[] { new Method(methodInfo), new Method(methodInfo) };

                var mixed = new[] {
                    new Method(typeof(TestType).GetMethod(nameof(TestType.Method1))),
                    expected[0],
                    expected[1],
                    new Method(typeof(TestType).GetMethod(nameof(TestType.Method3))),
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Method> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }

        static class TestType
        {
            public static void Method1(P p1) { }
            public static void Method2(P p1, P p2) { }
            public static void Method3(P p1, P p2, P p3) { }
        }

        class P { }
    }
}
