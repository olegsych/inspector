using System.Collections.Generic;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Inspector
{
    public class IEnumerableExtensionsTest: SelectorFixture<IEnumerableExtensionsTest.TestType>
    {
        public class TestType { }

        public class Single : IEnumerableExtensionsTest
        {
            [Fact]
            public void ReturnsValueReturnedBySelector() {
                var filter = Substitute.For<IEnumerable<TestType>>();
                var expected = new TestType();
                ConfiguredCall arrange = select.Invoke(filter).Returns(expected);

                TestType actual = filter.Single();

                Assert.Same(expected, actual);
            }
        }
    }
}
