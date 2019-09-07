using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    static class ParameterNameFilter
    {
        public static IEnumerable<ParameterInfo> WithName(this IEnumerable<ParameterInfo> parameters, string parameterName) =>
            new Implementation(parameters, parameterName);

        internal sealed class Implementation: Filter<ParameterInfo>
        {
            internal Implementation(IEnumerable<ParameterInfo> parameters, string parameterName) : base(parameters) =>
                ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));

            public string ParameterName { get; }

            public override IEnumerator<ParameterInfo> GetEnumerator() =>
                Source.Where(parameter => parameter.Name == ParameterName).GetEnumerator();
        }
    }
}
