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
        static readonly IDelegateFactory<ConstructorInfo> delegateFactory = new ConstructorDelegateFactory();

        #region IMembers

        public static Constructor Constructor(this IMembers members) =>
            members.Constructors().Single();

        public static Constructor Constructor(this IMembers members, Type delegateType) =>
            new ConstructorTypeFilter(members.Constructors(), delegateType, delegateFactory).Single();

        public static Constructor<TSignature> Constructor<TSignature>(this IMembers members) where TSignature : Delegate =>
            new Constructor<TSignature>(members.Constructor(typeof(TSignature)), delegateFactory);

        #endregion

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
