using System;
using System.Reflection;

namespace Inspector
{
    delegate bool DelegateFactory<T>(Type type, object target, T member, out Delegate @delegate) where T : MemberInfo;

    static class DelegateFactory
    {
        static readonly MethodInfo internalAlloc = typeof(Delegate).GetMethod("InternalAlloc", BindingFlags.Static | BindingFlags.NonPublic);
        static readonly MethodInfo bindToMethodInfo = typeof(Delegate).GetMethod("BindToMethodInfo", BindingFlags.Instance | BindingFlags.NonPublic);
        const byte RelaxedSignature = 0x80; // from internal DelegateBindingFlags

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
