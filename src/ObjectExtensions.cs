using System;
using System.Reflection;

namespace Inspector
{
    public static class ObjectExtensions
    {
        public static ObjectInspector Declared(this object instance)
            => throw new NotImplementedException();

        public static ObjectInspector Declared<TDeclaringType>(this object instance)
            => throw new NotImplementedException();

        #region Field

        public static Field Field(this object instance) =>
            new InstanceScope(instance).Field();

        public static Field Field(this object instance, string fieldName) =>
            new InstanceScope(instance).Field(fieldName);

        public static Field Field(this object instance, Type fieldType) =>
            new InstanceScope(instance).Field(fieldType);

        public static Field Field(this object instance, Type fieldType, string fieldName) =>
            new InstanceScope(instance).Field(fieldType, fieldName);

        public static Field<T> Field<T>(this object instance) =>
            new InstanceScope(instance).Field<T>();

        public static Field<T> Field<T>(this object instance, string fieldName) =>
            new InstanceScope(instance).Field<T>(fieldName);

        #endregion

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

        public static ObjectInspector Inherited(this object instance)
            => throw new NotImplementedException();

        public static ObjectInspector Inherited<TBaseType>(this object instance)
            => throw new NotImplementedException();

        public static Property<T> Property<T>(this object instance)
            => throw new NotImplementedException();

        static bool IsCastleDynamicProxy(TypeInfo instanceType)
            => instanceType.Assembly.GetName().Name == "DynamicProxyGenAssembly2";
    }
}
