using System;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting methods from a given scope.
    /// </summary>
    public static class MethodExtensions
    {
        #region IScope

        public static Method Method(this IScope scope) =>
            Selector<Method>.Select(scope);

        public static Method Method(this IScope scope, string methodName) =>
            Selector<Method>.Select(new MethodNameFilter(scope, methodName));

        public static Method Method(this IScope scope, Type methodType) =>
            Selector<Method>.Select(new MethodTypeFilter(scope, methodType, new MethodDelegateFactory()));

        public static Method Method(this IScope scope, Type methodType, string methodName) {
            var typed = new MethodTypeFilter(scope, methodType, new MethodDelegateFactory());
            var named = new MethodNameFilter(typed, methodName);
            return Selector<Method>.Select(named);
        }

        public static Method<T> Method<T>(this IScope scope) =>
            new Method<T>(scope.Method(typeof(T)));

        public static Method<T> Method<T>(this IScope scope, string methodName) =>
            new Method<T>(scope.Method(typeof(T), methodName));

        #endregion

        #region Object

        public static Method Method(this object instance) =>
            new InstanceScope(instance).Method();

        public static Method Method(this object instance, string methodName) =>
            new InstanceScope(instance).Method(methodName);

        public static Method Method(this object instance, Type methodType) =>
            new InstanceScope(instance).Method(methodType);

        public static Method Method(this object instance, Type methodType, string methodName) =>
            new InstanceScope(instance).Method(methodType, methodName);

        public static Method<T> Method<T>(this object instance) =>
            new InstanceScope(instance).Method<T>();

        public static Method<T> Method<T>(this object instance, string methodName) =>
            new InstanceScope(instance).Method<T>(methodName);

        #endregion

        #region Type

        public static Method Method(this Type type) =>
            new StaticScope(type).Method();

        public static Method Method(this Type type, Type methodType) =>
            new StaticScope(type).Method(methodType);

        public static Method Method(this Type type, string methodName) =>
            new StaticScope(type).Method(methodName);

        public static Method Method(this Type type, Type methodType, string methodName) =>
            new StaticScope(type).Method(methodType, methodName);

        public static Method<T> Method<T>(this Type type) =>
            new StaticScope(type).Method<T>();

        public static Method<T> Method<T>(this Type type, string methodName) =>
            new StaticScope(type).Method<T>(methodName);

        #endregion
    }
}
