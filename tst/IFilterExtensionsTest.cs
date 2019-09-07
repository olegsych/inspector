using NSubstitute;
using NSubstitute.Core;
using Xunit;
namespace Inspector
{
    public class IFilterExtensionsTest: SelectorFixture<IFilterExtensionsTest.TestType>
    {
        public class TestType { }

        public class Single : IFilterExtensionsTest
        {
            [Fact]
            public void ReturnsValueReturnedBySelector() {
                var filter = Substitute.For<IFilter<TestType>>();
                var expected = new TestType();
                ConfiguredCall arrange = select.Invoke(filter).Returns(expected);

                TestType actual = filter.Single();

                Assert.Same(expected, actual);
            }
        }
    }
}
