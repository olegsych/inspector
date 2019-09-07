using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Inspector.Implementation
{
    public class ConstructorTypeFilterTest
    {
        readonly Filter<Constructor> sut;

        // Constructor parameters
        readonly IEnumerable<Constructor> source = Substitute.For<IEnumerable<Constructor>>();
        readonly Type delegateType = typeof(Action<P, P>);
        readonly IDelegateFactory<ConstructorInfo> delegateFactory = Substitute.For<IDelegateFactory<ConstructorInfo>>();

        public ConstructorTypeFilterTest() =>
            sut = new ConstructorTypeFilter(source, delegateType, delegateFactory);

        public class ConstructorTest: ConstructorTypeFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenSourceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new ConstructorTypeFilter(null, delegateType, delegateFactory));
                Assert.Equal(nameof(source), thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new ConstructorTypeFilter(source, null, delegateFactory));
                Assert.Equal(nameof(delegateType), thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFactoryIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new ConstructorTypeFilter(source, delegateType, null));
                Assert.Equal(nameof(delegateFactory), thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsNotDelegate() {
                Type invalid = typeof(InvalidDelegateType);
                var thrown = Assert.Throws<ArgumentException>(() => new ConstructorTypeFilter(source, invalid, delegateFactory));
                Assert.Equal(nameof(delegateType), thrown.ParamName);
                Assert.StartsWith($"{invalid} is not a delegate.", thrown.Message);
            }

            [Fact]
            public void PassesSourceToBaseConstructor() =>
                Assert.Same(source, sut.Source);

            class InvalidDelegateType { }
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

        public class GetEnumerator: ConstructorTypeFilterTest
        {
            [Fact]
            public void ReturnsConstructorsWithGivenDelegateType() {
                ConfiguredCall arrange;
                var target = new TestType();
                ConstructorInfo[] infos = typeof(TestType).GetConstructors();
                arrange = delegateFactory.TryCreate(delegateType, target, infos[1], out Delegate @delegate).Returns(true);
                arrange = delegateFactory.TryCreate(delegateType, target, infos[3], out @delegate).Returns(true);
                List<Constructor> constructors = infos.Select(_ => new Constructor(_, target)).ToList();
                arrange = source.GetEnumerator().Returns(constructors.GetEnumerator());

                Constructor[] expected = { constructors[1], constructors[3] };
                Assert.Equal(expected, sut);
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
