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
    }
}
