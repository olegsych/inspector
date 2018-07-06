using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Inspector
{
    class DelegateFactory
    {
        static readonly MethodInfo internalAlloc = typeof(Delegate).GetMethod("InternalAlloc", BindingFlags.Static | BindingFlags.NonPublic);
        static readonly MethodInfo bindToMethodInfo = typeof(Delegate).GetMethod("BindToMethodInfo", BindingFlags.Instance | BindingFlags.NonPublic);

        // DelegateBindingFlags
        const byte RelaxedSignature = 0x80;

        internal static bool TryCreate(Type type, object target, ConstructorInfo constructor, out Delegate @delegate) {
            if(type == null)
                throw new ArgumentNullException(nameof(type));
            if(constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            var candidate = (Delegate)internalAlloc.Invoke(null, new object[] { type });
            var bound = (bool)bindToMethodInfo.Invoke(candidate, new object[] { target, constructor, constructor.DeclaringType, RelaxedSignature });
            @delegate = bound ? candidate : null;
            return bound;
        }

        internal static bool TryCreate(Type type, object target, MethodInfo method, out Delegate @delegate) {
            @delegate = Delegate.CreateDelegate(type, target, method, false);
            return @delegate != null;
        }
    }
}
