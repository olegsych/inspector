using System;
using System.Reflection;

namespace Inspector
{
    public static class ObjectExtensions
    {
        public static IScope Declared(this object instance)
            => throw new NotImplementedException();

        public static IScope Declared<TDeclaringType>(this object instance)
            => throw new NotImplementedException();

        #region Accessibility

        public static IScope Internal(this object instance) =>
            new InstanceScope(instance).Internal();

        public static IScope Private(this object instance) =>
            new InstanceScope(instance).Private();

        public static IScope Protected(this object instance) =>
            new InstanceScope(instance).Protected();

        public static IScope Public(this object instance) =>
            new InstanceScope(instance).Public();

        #endregion

        public static IScope Inherited(this object instance)
            => throw new NotImplementedException();

        public static IScope Inherited<TBaseType>(this object instance)
            => throw new NotImplementedException();

        public static Property<T> Property<T>(this object instance)
            => throw new NotImplementedException();

        static bool IsCastleDynamicProxy(TypeInfo instanceType)
            => instanceType.Assembly.GetName().Name == "DynamicProxyGenAssembly2";
    }
}
