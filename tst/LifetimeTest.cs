using System.Reflection;
using Xunit;

namespace Inspector
{
    public class LifetimeTest
    {
        [Theory]
        [InlineData(BindingFlags.Static, Lifetime.Static)]
        [InlineData(BindingFlags.Instance, Lifetime.Instance)]
        internal void ElementsMatchBindingFlags(BindingFlags expected, Lifetime actual) =>
            Assert.Equal((int)expected, (int)actual);
    }
}
