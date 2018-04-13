using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class MethodTypeFilter : IFilter<Method>, IDecorator<IFilter<Method>>
    {
        public MethodTypeFilter(IFilter<Method> previous, Type methodType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));

            if(methodType == null)
                throw new ArgumentNullException(nameof(methodType));
            if(!typeof(Delegate).IsAssignableFrom(methodType))
                throw new ArgumentException($"{methodType} is not a delegate.", nameof(methodType));
            MethodType = methodType;
        }

        public IFilter<Method> Previous { get; }

        public Type MethodType { get; }

        IEnumerable<Method> IFilter<Method>.Get() =>
            Previous.Get().Where(method => Delegate.CreateDelegate(MethodType, method.Info, false) != null);
    }
}
