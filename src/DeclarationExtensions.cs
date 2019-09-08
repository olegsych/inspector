using System;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for filtering type members based on their declaring type.
    /// </summary>
    public static class DeclarationExtensions
    {
        #region Object

        public static IMembers Declared(this object instance) =>
            new InstanceMembers(instance).DeclaredBy(instance.GetType());

        public static IMembers DeclaredBy(this object instance, Type declaringType) =>
            new InstanceMembers(instance).DeclaredBy(declaringType);

        public static IMembers DeclaredBy<T>(this object instance) =>
            new InstanceMembers(instance).DeclaredBy<T>();

        #endregion
    }
}
