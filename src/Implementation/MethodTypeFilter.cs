using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class MethodTypeFilter: Filter<Method>
    {
        public MethodTypeFilter(IEnumerable<Method> previous, Type delegateType, IDelegateFactory<MethodInfo> delegateFactory): base(previous) {
            if(delegateType == null)
                throw new ArgumentNullException(nameof(delegateType));
            if(!typeof(Delegate).IsAssignableFrom(delegateType))
                throw new ArgumentException($"{delegateType} is not a delegate.", nameof(delegateType));
            DelegateType = delegateType;

            DelegateFactory = delegateFactory ?? throw new ArgumentNullException(nameof(delegateFactory));
        }

        public Type DelegateType { get; }

        public IDelegateFactory<MethodInfo> DelegateFactory { get; }

        public override IEnumerator<Method> GetEnumerator() =>
            Previous.Where(method => DelegateFactory.TryCreate(DelegateType, method.Instance, method.Info, out Delegate _)).GetEnumerator();
    }
}
