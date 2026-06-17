using System;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class MethodDelegateFactory: IDelegateFactory<MethodInfo>
    {
        bool IDelegateFactory<MethodInfo>.TryCreate(Type delegateType, object? target, MethodInfo method, out Delegate? @delegate) {
            if(delegateType == null)
                throw new ArgumentNullException(nameof(delegateType)); // use descriptive ParamName
            try {
                @delegate = Delegate.CreateDelegate(delegateType, target, method, false);
                return @delegate != null;
            }
            catch(ArgumentException) {
                @delegate = null;
                return false;
            }
        }
    }
}
