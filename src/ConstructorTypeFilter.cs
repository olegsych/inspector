using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Filters constructors by type.
    /// </summary>
    sealed class ConstructorTypeFilter: IFilter<Constructor>, IDecorator<IFilter<Constructor>>
    {
        public ConstructorTypeFilter(IFilter<Constructor> previous, Type delegateType, IDelegateFactory<ConstructorInfo> delegateFactory) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));

            if(delegateType == null)
                throw new ArgumentNullException(nameof(delegateType));
            if(!typeof(Delegate).IsAssignableFrom(delegateType))
                throw new ArgumentException($"{delegateType} is not a delegate.", nameof(delegateType));
            DelegateType = delegateType;

            DelegateFactory = delegateFactory ?? throw new ArgumentNullException(nameof(delegateFactory));
        }

        public IFilter<Constructor> Previous { get; }
        public Type DelegateType { get; }
        public IDelegateFactory<ConstructorInfo> DelegateFactory { get; }

        IEnumerable<Constructor> IFilter<Constructor>.Get() =>
            Previous.Get().Where(constructor => DelegateFactory.TryCreate(DelegateType, constructor.Instance, constructor.Info, out Delegate _));
    }
}
