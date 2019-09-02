using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class ParameterTypeFilter: IFilter<ParameterInfo>, IDecorator<IFilter<ParameterInfo>>
    {
        public ParameterTypeFilter(IFilter<ParameterInfo> previous, Type parameterType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
        }

        public Type ParameterType { get; }

        public IFilter<ParameterInfo> Previous { get; }

        IEnumerable<ParameterInfo> IFilter<ParameterInfo>.Get() =>
            Previous.Get().Where(parameter => parameter.ParameterType == ParameterType);
    }
}
