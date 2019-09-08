using System;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for filtering type members based on their declaring type.
    /// </summary>
    public static class DeclarationExtensions
    {
        #region IMembers

        public static IMembers DeclaredBy(this IMembers members, Type declaringType) =>
            new DeclaredMembers(members, declaringType);

        public static IMembers DeclaredBy<T>(this IMembers members) =>
            new DeclaredMembers(members, typeof(T));

        #endregion

        #region Object

        public static IMembers Declared(this object instance) =>
            new InstanceMembers(instance).DeclaredBy(instance.GetType());

        public static IMembers DeclaredBy(this object instance, Type declaringType) =>
            new InstanceMembers(instance).DeclaredBy(declaringType);

        public static IMembers DeclaredBy<T>(this object instance) =>
            new InstanceMembers(instance).DeclaredBy<T>();

        #endregion

        #region Type

        public static IMembers Declared(this Type type) =>
            new StaticMembers(type).DeclaredBy(type);

        public static IMembers DeclaredBy(this Type type, Type declaringType) =>
            new StaticMembers(type).DeclaredBy(declaringType);

        public static IMembers DeclaredBy<T>(this Type type) =>
            new StaticMembers(type).DeclaredBy<T>();

        #endregion
    }
}
