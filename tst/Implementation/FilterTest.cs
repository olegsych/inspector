using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Inspector.Implementation
{
    public class FilterTest
    {
        public class TestType { }

        readonly IEnumerable<TestType> sut;

        // Constructor parameters
        readonly IEnumerable<TestType> previous = Substitute.For<IEnumerable<TestType>>();

        public FilterTest() =>
            sut = Substitute.ForPartsOf<Filter<TestType>>(previous);

        public class Constructor: FilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousIsNull() {
                var thrown = Assert.Throws<TargetInvocationException>(() => Substitute.ForPartsOf<Filter<TestType>>(new object[] { null }));
                var actual = Assert.IsType<ArgumentNullException>(thrown.InnerException);
                Assert.Equal("previous", actual.ParamName);
            }
        }

        public class Previous: FilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IEnumerable<TestType>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class GetEnumerator: FilterTest
        {
            [Fact]
            public void ReturnsStronglyTypedEnumerator() {
                IEnumerator<TestType> expected = Substitute.For<IEnumerator<TestType>>();
                ConfiguredCall arrange = sut.GetEnumerator().Returns(expected);

                IEnumerator actual = ((IEnumerable)sut).GetEnumerator();

                Assert.Same(expected, actual);
            }
        }
    }
}
