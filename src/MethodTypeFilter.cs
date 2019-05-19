using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Filters methods by type.
    /// </summary>
    sealed class MethodTypeFilter: IFilter<Method>, IDecorator<IFilter<Method>>
    {
        public MethodTypeFilter(IFilter<Method> previous, Type delegateType, IDelegateFactory<MethodInfo> delegateFactory) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));

            if(delegateType == null)
                throw new ArgumentNullException(nameof(delegateType));
            if(!typeof(Delegate).IsAssignableFrom(delegateType))
                throw new ArgumentException($"{delegateType} is not a delegate.", nameof(delegateType));
            DelegateType = delegateType;

            DelegateFactory = delegateFactory ?? throw new ArgumentNullException(nameof(delegateFactory));
        }

        public IFilter<Method> Previous { get; }
        public Type DelegateType { get; }
        public IDelegateFactory<MethodInfo> DelegateFactory { get; }

        IEnumerable<Method> IFilter<Method>.Get() =>
            Previous.Get().Where(method => DelegateFactory.TryCreate(DelegateType, method.Instance, method.Info, out Delegate _));
    }
}
