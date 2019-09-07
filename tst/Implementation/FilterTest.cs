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
        readonly TestableFilter<TestType> sut;

        // Constructor parameters
        readonly IEnumerable<TestType> source = Substitute.For<IEnumerable<TestType>>();

        public FilterTest() =>
            sut = Substitute.ForPartsOf<TestableFilter<TestType>>(source);

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
                IDecorator<IEnumerable<TestType>> decorator = sut;
                Assert.Same(source, decorator.Source);
            }
        }

        public class GetEnumerator: FilterTest
        {
            readonly IEnumerator<TestType> filteredEnumerator = Substitute.For<IEnumerator<TestType>>();

            public GetEnumerator() {
                IEnumerable<TestType> filtered = Substitute.For<IEnumerable<TestType>>();
                ConfiguredCall arrange = sut.TestableWhere().Returns(filtered);
                arrange = filtered.GetEnumerator().Returns(filteredEnumerator);
            }

            [Fact]
            public void ImplementsStronglyTypedIEnumerable() {
                IEnumerable<TestType> typed = sut;
                Assert.Same(filteredEnumerator, typed.GetEnumerator());
            }

            [Fact]
            public void ImplementsUntypedIEnumerable() {
                IEnumerable untyped = sut;
                Assert.Same(filteredEnumerator, untyped.GetEnumerator());
            }
        }

        public class TestType { }

        internal abstract class TestableFilter<T>: Filter<T>
        {
            protected TestableFilter(IEnumerable<T> source) : base(source) { }
            protected override IEnumerable<T> Where() => TestableWhere();
            public abstract IEnumerable<T> TestableWhere();
        }
    }
}
