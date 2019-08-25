using System;
using NSubstitute;
using Xunit;

namespace Inspector.Implementation
{
    public class SelectorTest
    {
        public class Select: SelectorTest
        {
            readonly IFilter<TestType> filter = Substitute.For<IFilter<TestType>>();

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => Selector<TestType>.Select(null));
                Assert.Equal("filter", thrown.ParamName);
            }

            [Fact]
            public void ReturnsSingleInstanceGottenFromFilter() {
                var expected = new TestType();
                filter.Get().Returns(new[] { expected });

                TestType actual = Selector<TestType>.Select(filter);

                Assert.Same(expected, actual);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFilterReturnsNoItems() {
                var thrown = Assert.Throws<InvalidOperationException>(() => Selector<TestType>.Select(filter));
                // TODO: verify thrown.Message
            }
        }

        public class TestType { }
    }
}
