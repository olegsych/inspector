using System;

namespace Inspector
{
    public static class ObjectMethodExtensions
    {
        public static Action Action(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Action<T> Action<T>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Action<T1, T2> Action<T1, T2>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Action<T1, T2, T3> Action<T1, T2, T3>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Action<T1, T2, T3, T4> Action<T1, T2, T3, T4>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Action<T1, T2, T3, T4, T5> Action<T1, T2, T3, T4, T5>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Action<T1, T2, T3, T4, T5, T6> Action<T1, T2, T3, T4, T5, T6>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Action<T1, T2, T3, T4, T5, T6, T7> Action<T1, T2, T3, T4, T5, T6, T7>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> Action<T1, T2, T3, T4, T5, T6, T7, T8>(this object instance, string methodName = null) => throw new NotImplementedException();

        public static Func<TResult> Func<TResult>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Func<T, TResult> Func<T, TResult>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Func<T1, T2, TResult> Func<T1, T2, TResult>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Func<T1, T2, T3, TResult> Func<T1, T2, T3, TResult>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Func<T1, T2, T3, T4, TResult> Func<T1, T2, T3, T4, TResult>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Func<T1, T2, T3, T4, T5, TResult> Func<T1, T2, T3, T4, T5, TResult>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Func<T1, T2, T3, T4, T5, T6, TResult> Func<T1, T2, T3, T4, T5, T6, TResult>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Func<T1, T2, T3, T4, T5, T6, T7, TResult>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this object instance, string methodName = null) => throw new NotImplementedException();

        public static T Method<T>(this object instance, string methodName = null) => throw new NotImplementedException();
        public static Delegate Method(this object instance, string methodName) => throw new NotImplementedException();
        public static Delegate Method(this object instance, params Type[] parameterTypes) => throw new NotImplementedException();
    }
}
