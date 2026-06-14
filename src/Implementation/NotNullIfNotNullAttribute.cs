// Polyfill a netstandard2.1 type to netstandard2.0 to prevent nullable warnings in net4xx callers.
// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.notnullifnotnullattribute

#pragma warning disable IDE0130 // Namespace must match the polyfilled type
namespace System.Diagnostics.CodeAnalysis;
#pragma warning restore IDE0130

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
sealed class NotNullIfNotNullAttribute(string parameterName): Attribute
{
    internal string ParameterName { get; } = parameterName;
}
