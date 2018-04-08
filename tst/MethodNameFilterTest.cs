using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class MethodNameFilterTest
    {
        readonly IFilter<Method> sut;

        // Constructor parameters
        readonly IFilter<Method> previous = Substitute.For<IFilter<Method>>();
        readonly string methodName = Guid.NewGuid().ToString();

        public MethodNameFilterTest() =>
            sut = new MethodNameFilter(previous, methodName);

        public class Constructor : MethodNameFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MethodNameFilter(null, methodName));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFieldNameIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MethodNameFilter(previous, null));
                Assert.Equal("methodName", thrown.ParamName);
            }
        }

        public class FieldName : MethodNameFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(methodName, ((MethodNameFilter)sut).MethodName);
        }

        public class Previous : MethodNameFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Method>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get : MethodNameFilterTest
        {
            [Fact]
            public void ReturnsMethodsWithGivenName() {
                // Arrange
                MethodInfo methodInfo = MethodInfo(MethodAttributes.Static, methodName);

                var expected = new[] { new Method(methodInfo), new Method(methodInfo) };

                var mixed = new[] {
                    new Method(MethodInfo(MethodAttributes.Static)),
                    expected[0],
                    new Method(MethodInfo(MethodAttributes.Static)),
                    expected[1],
                    new Method(MethodInfo(MethodAttributes.Static))
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Method> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
