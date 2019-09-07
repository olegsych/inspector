using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting constructors from a given scope.
    /// </summary>
    public static class ConstructorExtensions
    {
        static readonly IDelegateFactory<ConstructorInfo> delegateFactory = new ConstructorDelegateFactory();

        #region IScope

        public static Constructor Constructor(this IScope scope) =>
            scope.Constructors().Single();

        public static Constructor Constructor(this IScope scope, Type delegateType) =>
            new ConstructorTypeFilter(scope.Constructors(), delegateType, delegateFactory).Single();

        public static Constructor<TSignature> Constructor<TSignature>(this IScope scope) where TSignature : Delegate =>
            new Constructor<TSignature>(scope.Constructor(typeof(TSignature)), delegateFactory);

        #endregion

        #region Object

        public static Constructor Constructor(this object instance) =>
            instance.Declared().Constructor(); // Declared only because at least one constructor is always inherited from Object

        public static Constructor Constructor(this object instance, Type delegateType) =>
            new InstanceScope(instance).Constructor(delegateType);

        public static Constructor<TSignature> Constructor<TSignature>(this object instance) where TSignature : Delegate =>
            new InstanceScope(instance).Constructor<TSignature>();

        #endregion

        #region Type

        public static Constructor Constructor(this Type type) =>
            new StaticScope(type).Constructor();

        #endregion
    }
}
