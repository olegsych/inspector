using System;

namespace Inspector
{
    /// <summary>
    /// Extension methods for filtering type members based on their declaring type.
    /// </summary>
    public static class DeclarationExtensions
    {
        #region IScope

        public static IScope DeclaredBy(this IScope scope, Type declaringType) =>
            new DeclarationScope(scope, declaringType);

        public static IScope DeclaredBy<T>(this IScope scope) =>
            new DeclarationScope(scope, typeof(T));

        #endregion

        #region Object

        public static IScope Declared(this object instance) =>
            new InstanceScope(instance).DeclaredBy(instance.GetType());

        public static IScope DeclaredBy(this object instance, Type declaringType) =>
            new InstanceScope(instance).DeclaredBy(declaringType);

        public static IScope DeclaredBy<T>(this object instance) =>
            new InstanceScope(instance).DeclaredBy<T>();

        #endregion

        #region Type

        public static IScope Declared(this Type type) =>
            new StaticScope(type).DeclaredBy(type);

        public static IScope DeclaredBy(this Type type, Type declaringType) =>
            new StaticScope(type).DeclaredBy(declaringType);

        public static IScope DeclaredBy<T>(this Type type) =>
            new StaticScope(type).DeclaredBy<T>();

        #endregion
    }
}
