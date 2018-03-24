using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    public static partial class ObjectExtensions
    {
        public static ObjectInspector Declared(this object instance)
            => throw new NotImplementedException();

        public static ObjectInspector Declared<TDeclaringType>(this object instance)
            => throw new NotImplementedException();

        #region Field

        public static Field Field(this object instance, string fieldName = null) =>
            Field(instance, null, fieldName);

        public static Field Field(this object instance, Type fieldType, string fieldName = null) =>
            Inspector.Field.Select(new InstanceScope(instance), fieldType, fieldName);

        public static Field<T> Field<T>(this object instance, string fieldName = null) {
            Field field = Field(instance, typeof(T), fieldName);
            return new Field<T>(field.Info, field.Instance);
        }

        #endregion

        public static ObjectInspector Inherited(this object instance)
            => throw new NotImplementedException();

        public static ObjectInspector Inherited<TBaseType>(this object instance)
            => throw new NotImplementedException();

        public static ObjectInspector Internal(this object instance)
            => throw new NotImplementedException();

        public static ObjectInspector Private(this object instance)
            => throw new NotImplementedException();

        public static ObjectInspector Protected(this object instance)
            => throw new NotImplementedException();

        public static Property<T> Property<T>(this object instance)
            => throw new NotImplementedException();

        public static ObjectInspector Public(this object instance)
            => throw new NotImplementedException();

        static bool IsCastleDynamicProxy(TypeInfo instanceType)
            => instanceType.Assembly.GetName().Name == "DynamicProxyGenAssembly2";
    }
}
