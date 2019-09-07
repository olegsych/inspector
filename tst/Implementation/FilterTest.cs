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
        readonly IEnumerable<TestType> source = Substitute.For<IEnumerable<TestType>>();

        public FilterTest() =>
            sut = Substitute.ForPartsOf<Filter<TestType>>(source);

        public class Constructor: FilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenSourceIsNull() {
                var thrown = Assert.Throws<TargetInvocationException>(() => Substitute.ForPartsOf<Filter<TestType>>(new object[] { null }));
                var actual = Assert.IsType<ArgumentNullException>(thrown.InnerException);
                Assert.Equal("source", actual.ParamName);
            }
        }

        public class Source: FilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IEnumerable<TestType>>)sut;
                Assert.Same(source, decorator.Source);
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
