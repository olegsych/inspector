extern alias inspector;
using System.Reflection;
using Xunit;
using NotNullIfNotNull = inspector::System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute;

namespace System.Diagnostics.CodeAnalysis;

public abstract class NotNullIfNotNullAttributeTest
{
    readonly NotNullIfNotNull sut;
    readonly string parameterName = Guid.NewGuid().ToString();

    NotNullIfNotNullAttributeTest() =>
        sut = new NotNullIfNotNull(parameterName);

    public sealed class Constructor: NotNullIfNotNullAttributeTest
    {
        [Fact]
        public void InitializesParameterName() =>
            Assert.Same(parameterName, sut.ParameterName);
    }

    public sealed class AttributeUsage: NotNullIfNotNullAttributeTest
    {
        readonly AttributeUsageAttribute usage =
            typeof(NotNullIfNotNull).GetCustomAttribute<AttributeUsageAttribute>()!;

        [Fact]
        public void AllowsAttributeOnParameterPropertyAndReturnValue() =>
            Assert.Equal(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, usage.ValidOn);

        [Fact]
        public void AllowsMultipleAttributes() =>
            Assert.True(usage.AllowMultiple);

        [Fact(Explicit = true)] // TODO: SUT bug. NotNullIfNotNullAttribute should not be inherited.
        public void PreventsAttributeInheritance() =>
            Assert.False(usage.Inherited);
    }
}
