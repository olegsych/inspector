using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Inspector.Implementation
{
    public class SelectorTest
    {
        public class Select: SelectorTest
        {
            readonly IEnumerable<TestType> enumerable = Substitute.For<IEnumerable<TestType>>();

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => Selector<TestType>.Select(null!));
                Assert.Equal("filter", thrown.ParamName);
            }

            [Fact]
            public void ReturnsSingleInstanceGottenFromFilter() {
                var expected = new TestType();
                ConfiguredCall arrange = enumerable.GetEnumerator().Returns(new[] { expected }.Cast<TestType>().GetEnumerator());

                TestType actual = Selector<TestType>.Select(enumerable);

                Assert.Same(expected, actual);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFilterReturnsNoItems() {
                var thrown = Assert.Throws<InvalidOperationException>(() => Selector<TestType>.Select(enumerable));
                // TODO: verify thrown.Message
            }
        }

        public class TestType { }
    }
}
