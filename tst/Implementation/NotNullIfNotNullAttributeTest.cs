extern alias inspector;
using System;
using System.Reflection;
using Xunit;
using NotNullIfNotNullAttribute = inspector::System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute;

namespace Inspector.Implementation;

public abstract class NotNullIfNotNullAttributeTest
{
    readonly NotNullIfNotNullAttribute sut;

    // Constructor parameters
    readonly string parameterName = Guid.NewGuid().ToString();

    NotNullIfNotNullAttributeTest() =>
        sut = new NotNullIfNotNullAttribute(parameterName);

    public sealed class ParameterName: NotNullIfNotNullAttributeTest
    {
        [Fact]
        public void IsSetToGivenValue() =>
            Assert.Same(parameterName, sut.ParameterName);
    }

    public sealed class Usage: NotNullIfNotNullAttributeTest
    {
        readonly AttributeUsageAttribute usage =
            typeof(NotNullIfNotNullAttribute).GetCustomAttribute<AttributeUsageAttribute>()!;

        [Fact]
        public void MatchesPolyfilledAttribute() {
            Assert.Equal(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, usage.ValidOn);
            Assert.True(usage.AllowMultiple);
        }
    }
}
