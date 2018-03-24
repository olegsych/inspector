using System;
using Xunit;
namespace Inspector
{
    public class InstanceScopeTest
    {
        readonly IScope sut;

        // Constructor parameters
        readonly object instance = new object();

        // Fixture
        public InstanceScopeTest() =>
            sut = new InstanceScope(instance);

        public class Constructor : InstanceScopeTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenInstanceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new InstanceScope(null));
                Assert.Equal("instance", thrown.ParamName);
            }

            [Fact]
            public void InitializesInstanceProperty() =>
                Assert.Same(instance, ((InstanceScope)sut).Instance);
        }
    }
}
