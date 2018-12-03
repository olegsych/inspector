using System;
using System.Reflection;

namespace Inspector
{
    public static class ObjectExtensions
    {
        public static IScope Inherited(this object instance)
            => throw new NotImplementedException();

        public static IScope Inherited<TBaseType>(this object instance)
            => throw new NotImplementedException();

        static bool IsCastleDynamicProxy(TypeInfo instanceType)
            => instanceType.Assembly.GetName().Name == "DynamicProxyGenAssembly2";
    }
}
