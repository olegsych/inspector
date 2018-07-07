using System;
using System.Reflection;

namespace Inspector
{
    interface IDelegateFactory<T> where T : MethodBase
    {
        bool TryCreate(Type delegateType, object target, T method, out Delegate @delegate);
    }
}
