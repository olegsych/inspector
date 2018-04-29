using System;

namespace Inspector
{
    public static class ConstructorExtensions
    {
        #region IScope

        public static Constructor Constructor(this IScope scope) =>
            Selector<Constructor>.Select(scope);

        #endregion

        #region Object

        public static Constructor Constructor(this object instance) =>
            new InstanceScope(instance).Constructor();

        #endregion

        #region Type

        public static Constructor Constructor(this Type type) =>
            new StaticScope(type).Constructor();

        #endregion
    }
}
