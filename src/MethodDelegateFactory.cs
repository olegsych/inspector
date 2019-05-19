using System;
using System.Reflection;

namespace Inspector
{
    sealed class MethodDelegateFactory: IDelegateFactory<MethodInfo>
    {
        bool IDelegateFactory<MethodInfo>.TryCreate(Type delegateType, object target, MethodInfo method, out Delegate @delegate) {
            if(delegateType == null)
                throw new ArgumentNullException(nameof(delegateType)); // use descriptive ParamName
            @delegate = Delegate.CreateDelegate(delegateType, target, method, false);
            return @delegate != null;
        }
    }
}
