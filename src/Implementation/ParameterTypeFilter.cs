using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    public static class ParameterTypeFilter
    {
        public static IEnumerable<ParameterInfo> WithType(this IEnumerable<ParameterInfo> parameters, Type parameterType) =>
            new Implementation(parameters, parameterType);

        internal sealed class Implementation: Filter<ParameterInfo>
        {
            internal Implementation(IEnumerable<ParameterInfo> parameters, Type parameterType) : base(parameters) =>
                ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));

            public Type ParameterType { get; }

            public override IEnumerator<ParameterInfo> GetEnumerator() =>
                Source.Where(parameter => parameter.ParameterType == ParameterType).GetEnumerator();
        }
    }
}
