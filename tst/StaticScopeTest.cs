using System;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class StaticScopeTest
    {
        readonly IScope sut;

        readonly Type type = typeof(TestType);

        public StaticScopeTest() =>
            sut = new StaticScope(type);

        public class Constructor : StaticScopeTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenTypeIsNullToFailFast() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new StaticScope(null));
                Assert.Equal("type", thrown.ParamName);
            }

            [Fact]
            public void InitializesScopeWithGivenType() =>
                Assert.Same(type.GetTypeInfo(), ((StaticScope)sut).TypeInfo);
        }

        static class TestType { }
    }
}
