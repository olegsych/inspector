using System;

namespace Inspector
{
    /// <summary>
    /// Extension methods for filtering type members based on the ancestor type they are inherited from.
    /// </summary>
    public static class InheritanceExtensions
    {
        #region IScope

        public static IScope InheritedFrom(this IScope scope, Type ancestorType) =>
            new InheritanceScope(scope, ancestorType);

        public static IScope InheritedFrom<T>(this IScope scope) =>
            new InheritanceScope(scope, typeof(T));

        #endregion

        #region Object

        public static IScope Inherited(this object instance) =>
            new InstanceScope(instance).InheritedFrom(instance.GetType().BaseType);

        public static IScope InheritedFrom(this object instance, Type ancestorType) =>
            new InstanceScope(instance).InheritedFrom(ancestorType);

        public static IScope InheritedFrom<T>(this object instance) =>
            new InstanceScope(instance).InheritedFrom<T>();

        #endregion

        #region Type

        public static IScope Inherited(this Type type) =>
            new StaticScope(type).InheritedFrom(type.BaseType);

        public static IScope InheritedFrom(this Type type, Type ancestorType) =>
            new StaticScope(type).InheritedFrom(ancestorType);

        public static IScope InheritedFrom<T>(this Type type) =>
            new StaticScope(type).InheritedFrom<T>();

        #endregion
    }
}
