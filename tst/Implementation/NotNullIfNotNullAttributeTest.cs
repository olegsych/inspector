extern alias inspector;
using System;
using System.Reflection;
using Xunit;
using NotNullIfNotNull = inspector::System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute;

namespace System.Diagnostics.CodeAnalysis;

public abstract class NotNullIfNotNullAttributeTest
{
    readonly NotNullIfNotNull sut;

    // Constructor parameters
    readonly string parameterName = Guid.NewGuid().ToString();

    NotNullIfNotNullAttributeTest() =>
        sut = new NotNullIfNotNull(parameterName);

    public sealed class Constructor: NotNullIfNotNullAttributeTest
    {
        [Fact]
        public void InitializesParameterName() =>
            Assert.Same(parameterName, sut.ParameterName);
    }

    public sealed class Usage: NotNullIfNotNullAttributeTest
    {
        readonly AttributeUsageAttribute usage =
            typeof(NotNullIfNotNull).GetCustomAttribute<AttributeUsageAttribute>()!;

        [Fact]
        public void MatchesPolyfilledAttribute() {
            Assert.Equal(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, usage.ValidOn);
            Assert.True(usage.AllowMultiple);
        }
    }
}
