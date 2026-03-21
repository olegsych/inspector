using System;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class ConstructorDelegateFactory: IDelegateFactory<ConstructorInfo>
    {
        static readonly MethodInfo internalAlloc = typeof(Delegate).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
          .First(_ => _.Name == "InternalAlloc" && _.GetParameters().Length == 1); // .NET 9 has 2 overloads
        static readonly MethodInfo bindToMethodInfo = typeof(Delegate).GetMethod("BindToMethodInfo", BindingFlags.Instance | BindingFlags.NonPublic);
        const byte RelaxedSignature = 0x80; // from internal DelegateBindingFlags

        bool IDelegateFactory<ConstructorInfo>.TryCreate(Type delegateType, object? target, ConstructorInfo method, out Delegate? @delegate) {
            if(delegateType == null)
                throw new ArgumentNullException(nameof(delegateType));
            if(method == null)
                throw new ArgumentNullException(nameof(method));

            var candidate = (Delegate)internalAlloc.Invoke(null, new object[] { delegateType });
            var bound = (bool)bindToMethodInfo.Invoke(candidate, new object?[] { target, method, method.DeclaringType, RelaxedSignature });
            @delegate = bound ? candidate : null;
            return bound;
        }
    }
}
