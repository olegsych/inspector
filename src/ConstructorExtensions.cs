using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting constructors.
    /// </summary>
    public static class ConstructorExtensions
    {
        #region Object

        public static Constructor Constructor(this object instance) =>
            instance.Declared().Constructor(); // Declared only because at least one constructor is always inherited from Object

        public static Constructor Constructor(this object instance, Type delegateType) =>
            new InstanceMembers(instance).Constructor(delegateType);

        public static Constructor<TSignature> Constructor<TSignature>(this object instance) where TSignature : Delegate =>
            new InstanceMembers(instance).Constructor<TSignature>();

        #endregion

        #region Type

        public static Constructor Constructor(this Type type) =>
            new StaticMembers(type).Constructor();

        #endregion
    }
}
