using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class ConstructorTypeFilter: Filter<Constructor>
    {
        public ConstructorTypeFilter(IEnumerable<Constructor> previous, Type delegateType, IDelegateFactory<ConstructorInfo> delegateFactory): base(previous) {
            if(delegateType == null)
                throw new ArgumentNullException(nameof(delegateType));
            if(!typeof(Delegate).IsAssignableFrom(delegateType))
                throw new ArgumentException($"{delegateType} is not a delegate.", nameof(delegateType));
            DelegateType = delegateType;

            DelegateFactory = delegateFactory ?? throw new ArgumentNullException(nameof(delegateFactory));
        }

        public Type DelegateType { get; }

        public IDelegateFactory<ConstructorInfo> DelegateFactory { get; }

        public override IEnumerator<Constructor> GetEnumerator() =>
            Previous.Where(constructor => DelegateFactory.TryCreate(DelegateType, constructor.Instance, constructor.Info, out Delegate _)).GetEnumerator();
    }
}
