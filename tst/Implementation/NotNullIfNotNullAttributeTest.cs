extern alias inspector;

using System;
using System.Reflection;
using Xunit;
using PolyfillAttribute = inspector::System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute;

namespace Inspector.Implementation;

public abstract class NotNullIfNotNullAttributeTest
{
    readonly PolyfillAttribute sut;
    readonly string parameterName = Guid.NewGuid().ToString();

    NotNullIfNotNullAttributeTest() => sut = new PolyfillAttribute(parameterName);

    public sealed class Constructor: NotNullIfNotNullAttributeTest
    {
        [Fact]
        public void InitializesParameterName() => Assert.Same(parameterName, sut.ParameterName);
    }

    public sealed class AttributeUsage: NotNullIfNotNullAttributeTest
    {
        [Fact]
        public void MatchesBcl()
        {
            AttributeUsageAttribute usage = typeof(PolyfillAttribute).GetCustomAttribute<AttributeUsageAttribute>()!;
            Assert.Equal(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, usage.ValidOn);
            Assert.True(usage.AllowMultiple);
            Assert.False(usage.Inherited);
        }
    }
}
