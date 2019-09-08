using System;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for filtering type members based on the ancestor type they are inherited from.
    /// </summary>
    public static class InheritanceExtensions
    {
        #region Object

        public static IMembers Inherited(this object instance) =>
            new InstanceMembers(instance).InheritedFrom(instance.GetType().BaseType);

        public static IMembers InheritedFrom(this object instance, Type ancestorType) =>
            new InstanceMembers(instance).InheritedFrom(ancestorType);

        public static IMembers InheritedFrom<T>(this object instance) =>
            new InstanceMembers(instance).InheritedFrom<T>();

        #endregion

        #region Type

        public static IMembers Inherited(this Type type) =>
            new StaticMembers(type).InheritedFrom(type.BaseType);

        public static IMembers InheritedFrom(this Type type, Type ancestorType) =>
            new StaticMembers(type).InheritedFrom(ancestorType);

        public static IMembers InheritedFrom<T>(this Type type) =>
            new StaticMembers(type).InheritedFrom<T>();

        #endregion
    }
}
