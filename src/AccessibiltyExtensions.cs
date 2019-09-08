using System;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for filtering type members based on their <see cref="Accessibility"/>.
    /// </summary>
    public static class AccessibiltyExtensions
    {
        #region Object

        public static IMembers Internal(this object instance) =>
            new InstanceMembers(instance).Internal();

        public static IMembers Private(this object instance) =>
            new InstanceMembers(instance).Private();

        public static IMembers Protected(this object instance) =>
            new InstanceMembers(instance).Protected();

        public static IMembers Public(this object instance) =>
            new InstanceMembers(instance).Public();

        #endregion

        #region Type

        public static IMembers Internal(this Type type) =>
            new StaticMembers(type).Internal();

        public static IMembers Private(this Type type) =>
            new StaticMembers(type).Private();

        public static IMembers Protected(this Type type) =>
            new StaticMembers(type).Protected();

        public static IMembers Public(this Type type) =>
            new StaticMembers(type).Public();

        #endregion
    }
}
