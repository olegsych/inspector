using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    public static class ParameterTypeFilter
    {
        public static IFilter<ParameterInfo> WithType(this IFilter<ParameterInfo> parameters, Type parameterType) =>
            new Implementation(parameters, parameterType);

        internal sealed class Implementation: IFilter<ParameterInfo>, IDecorator<IFilter<ParameterInfo>>
        {
            internal Implementation(IFilter<ParameterInfo> parameters, Type parameterType) {
                Previous = parameters ?? throw new ArgumentNullException(nameof(parameters));
                ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            }

            public Type ParameterType { get; }

            public IFilter<ParameterInfo> Previous { get; }

            IEnumerable<ParameterInfo> IFilter<ParameterInfo>.Get() =>
                Previous.Get().Where(parameter => parameter.ParameterType == ParameterType);
        }
    }
}
