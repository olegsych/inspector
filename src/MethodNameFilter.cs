using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class MethodNameFilter : IFilter<Method>, IDecorator<IFilter<Method>>
    {
        public MethodNameFilter(IFilter<Method> previous, string methodName) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
        }

        public IFilter<Method> Previous { get; }

        public string MethodName { get; }

        string IDescriptor.Describe() =>
            throw new NotImplementedException();

        IEnumerable<Method> IFilter<Method>.Get() =>
            Previous.Get().Where(method => method.Info.Name == MethodName);
    }
}
