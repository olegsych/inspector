using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    static class ParameterNameFilter
    {
        public static IFilter<ParameterInfo> WithName(this IFilter<ParameterInfo> parameters, string parameterName) =>
            new Implementation(parameters, parameterName);

        internal sealed class Implementation: IFilter<ParameterInfo>, IDecorator<IFilter<ParameterInfo>>
        {
            internal Implementation(IFilter<ParameterInfo> parameters, string parameterName) {
                Previous = parameters ?? throw new ArgumentNullException(nameof(parameters));
                ParameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
            }

            public string ParameterName { get; }

            public IFilter<ParameterInfo> Previous { get; }

            IEnumerable<ParameterInfo> IFilter<ParameterInfo>.Get() =>
                Previous.Get().Where(parameter => parameter.Name == ParameterName);
        }
    }
}
