extern alias inspector;
using System.Reflection;
using Xunit;
using PolyfillAttribute = inspector::System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute;

#pragma warning disable IDE0130 // Namespace must match the polyfilled type
namespace System.Diagnostics.CodeAnalysis;
#pragma warning restore IDE0130

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
