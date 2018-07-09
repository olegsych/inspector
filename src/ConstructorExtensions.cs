using System;
using System.Reflection;

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
            Selector<Constructor>.Select(scope);

        public static Constructor Constructor(this IScope scope, Type delegateType) =>
            Selector<Constructor>.Select(new ConstructorTypeFilter(scope, delegateType, delegateFactory));

        public static Constructor<TSignature> Constructor<TSignature>(this IScope scope) where TSignature : Delegate =>
            new Constructor<TSignature>(scope.Constructor(typeof(TSignature)), delegateFactory);

        #endregion

        #region Object

        public static Constructor Constructor(this object instance) =>
            new InstanceScope(instance).Constructor();

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
