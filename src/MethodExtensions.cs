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
    }
}
