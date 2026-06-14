#pragma warning disable IDE0130 // Namespace must match the polyfilled type
namespace System.Diagnostics.CodeAnalysis;
#pragma warning restore IDE0130

// Polyfill https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.notnullifnotnullattribute
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true)]
sealed class NotNullIfNotNullAttribute(string parameterName): Attribute
{
    internal string ParameterName { get; } = parameterName;
}
