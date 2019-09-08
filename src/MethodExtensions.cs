using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting methods.
    /// </summary>
    public static class MethodExtensions
    {
        static readonly IDelegateFactory<MethodInfo> delegateFactory = new MethodDelegateFactory();

        #region IMembers

        public static Method Method(this IMembers members) =>
            members.Methods().Single();

        public static Method Method(this IMembers members, string methodName) =>
            new MemberNameFilter<Method, MethodInfo>(members.Methods(), methodName).Single();

        public static Method Method(this IMembers members, Type methodType) =>
            new MethodTypeFilter(members.Methods(), methodType, delegateFactory).Single();

        public static Method Method(this IMembers members, Type methodType, string methodName) {
            var typed = new MethodTypeFilter(members.Methods(), methodType, delegateFactory);
            var named = new MemberNameFilter<Method, MethodInfo>(typed, methodName);
            return named.Single();
        }

        public static Method<T> Method<T>(this IMembers members) where T : Delegate =>
            new Method<T>(members.Method(typeof(T)), delegateFactory);

        public static Method<T> Method<T>(this IMembers members, string methodName) where T : Delegate =>
            new Method<T>(members.Method(typeof(T), methodName), delegateFactory);

        #endregion

        #region Object

        public static Method Method(this object instance) =>
            instance.Declared().Method(); // Declared only because multiple methods are always inherited from Object

        public static Method Method(this object instance, string methodName) =>
            new InstanceMembers(instance).Method(methodName);

        public static Method Method(this object instance, Type methodType) =>
            new InstanceMembers(instance).Method(methodType);

        public static Method Method(this object instance, Type methodType, string methodName) =>
            new InstanceMembers(instance).Method(methodType, methodName);

        public static Method<T> Method<T>(this object instance) where T : Delegate =>
            new InstanceMembers(instance).Method<T>();

        public static Method<T> Method<T>(this object instance, string methodName) where T : Delegate =>
            new InstanceMembers(instance).Method<T>(methodName);

        #endregion

        #region Type

        public static Method Method(this Type type) =>
            new StaticMembers(type).Method();

        public static Method Method(this Type type, Type methodType) =>
            new StaticMembers(type).Method(methodType);

        public static Method Method(this Type type, string methodName) =>
            new StaticMembers(type).Method(methodName);

        public static Method Method(this Type type, Type methodType, string methodName) =>
            new StaticMembers(type).Method(methodType, methodName);

        public static Method<T> Method<T>(this Type type) where T : Delegate =>
            new StaticMembers(type).Method<T>();

        public static Method<T> Method<T>(this Type type, string methodName) where T : Delegate =>
            new StaticMembers(type).Method<T>(methodName);

        #endregion
    }
}
