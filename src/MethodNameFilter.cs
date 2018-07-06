using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    /// <summary>
    /// Filters methods by name.
    /// </summary>
    sealed class MethodNameFilter : IFilter<Method>, IDecorator<IFilter<Method>>
    {
        public MethodNameFilter(IFilter<Method> previous, string methodName) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
        }

        public IFilter<Method> Previous { get; }

        public string MethodName { get; }

        IEnumerable<Method> IFilter<Method>.Get() =>
            Previous.Get().Where(method => method.Info.Name == MethodName);
    }
}
