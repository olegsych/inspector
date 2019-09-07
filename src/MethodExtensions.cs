using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting methods from a given scope.
    /// </summary>
    public static class MethodExtensions
    {
        static readonly IDelegateFactory<MethodInfo> delegateFactory = new MethodDelegateFactory();

        #region IScope

        public static Method Method(this IScope scope) =>
            scope.Methods().Single();

        public static Method Method(this IScope scope, string methodName) =>
            new MemberNameFilter<Method, MethodInfo>(scope.Methods(), methodName).Single<Method>();

        public static Method Method(this IScope scope, Type methodType) =>
            new MethodTypeFilter(scope.Methods(), methodType, delegateFactory).Single<Method>();

        public static Method Method(this IScope scope, Type methodType, string methodName) {
            var typed = new MethodTypeFilter(scope.Methods(), methodType, delegateFactory);
            var named = new MemberNameFilter<Method, MethodInfo>(typed, methodName);
            return named.Single();
        }

        public static Method<T> Method<T>(this IScope scope) where T : Delegate =>
            new Method<T>(scope.Method(typeof(T)), delegateFactory);

        public static Method<T> Method<T>(this IScope scope, string methodName) where T : Delegate =>
            new Method<T>(scope.Method(typeof(T), methodName), delegateFactory);

        #endregion

        #region Object

        public static Method Method(this object instance) =>
            instance.Declared().Method(); // Declared only because multiple methods are always inherited from Object

        public static Method Method(this object instance, string methodName) =>
            new InstanceScope(instance).Method(methodName);

        public static Method Method(this object instance, Type methodType) =>
            new InstanceScope(instance).Method(methodType);

        public static Method Method(this object instance, Type methodType, string methodName) =>
            new InstanceScope(instance).Method(methodType, methodName);

        public static Method<T> Method<T>(this object instance) where T : Delegate =>
            new InstanceScope(instance).Method<T>();

        public static Method<T> Method<T>(this object instance, string methodName) where T : Delegate =>
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

        public static Method<T> Method<T>(this Type type) where T : Delegate =>
            new StaticScope(type).Method<T>();

        public static Method<T> Method<T>(this Type type, string methodName) where T : Delegate =>
            new StaticScope(type).Method<T>(methodName);

        #endregion
    }
}
