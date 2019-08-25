using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector.Implementation
{
    public class ConstructorTypeFilterTest
    {
        readonly IFilter<Constructor> sut;

        // Constructor parameters
        readonly IFilter<Constructor> previous = Substitute.For<IFilter<Constructor>>();
        readonly Type delegateType = typeof(Action<P, P>);
        readonly IDelegateFactory<ConstructorInfo> delegateFactory = Substitute.For<IDelegateFactory<ConstructorInfo>>();

        public ConstructorTypeFilterTest() =>
            sut = new ConstructorTypeFilter(previous, delegateType, delegateFactory);

        public class ConstructorTest: ConstructorTypeFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new ConstructorTypeFilter(null, delegateType, delegateFactory));
                Assert.Equal(nameof(previous), thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new ConstructorTypeFilter(previous, null, delegateFactory));
                Assert.Equal(nameof(delegateType), thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFactoryIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new ConstructorTypeFilter(previous, delegateType, null));
                Assert.Equal(nameof(delegateFactory), thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsNotDelegate() {
                Type invalid = typeof(InvalidDelegateType);
                var thrown = Assert.Throws<ArgumentException>(() => new ConstructorTypeFilter(previous, invalid, delegateFactory));
                Assert.Equal(nameof(delegateType), thrown.ParamName);
                Assert.StartsWith($"{invalid} is not a delegate.", thrown.Message);
            }

            class InvalidDelegateType { }
        }

        public class Previous: ConstructorTypeFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Constructor>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class DelegateType: ConstructorTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(delegateType, ((ConstructorTypeFilter)sut).DelegateType);
        }

        public class DelegateFactory: ConstructorTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(delegateFactory, ((ConstructorTypeFilter)sut).DelegateFactory);
        }

        public class Get: ConstructorTypeFilterTest
        {
            [Fact]
            public void ReturnsConstructorsWithGivenDelegateType() {
                // Arrange
                ConstructorInfo[] infos = typeof(TestType).GetConstructors();
                var target = new TestType();
                delegateFactory.TryCreate(delegateType, target, infos[1], out Delegate @delegate).Returns(true);
                delegateFactory.TryCreate(delegateType, target, infos[3], out @delegate).Returns(true);

                Constructor[] constructors = infos.Select(_ => new Constructor(_, target)).ToArray();
                previous.Get().Returns(constructors);

                // Act
                IEnumerable<Constructor> actual = sut.Get();

                // Assert
                Constructor[] expected = { constructors[1], constructors[3] };
                Assert.Equal(expected, actual);
            }
        }

        class TestType
        {
            public TestType() { }
            public TestType(P p1) { }
            public TestType(P p1, P p2) { }
            public TestType(P p1, P p2, P p3) { }
            public TestType(P p1, P p2, P p3, P p4) { }
        }

        class P { }
    }
}
