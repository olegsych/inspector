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

        public static Field Field(this object instance) {
            var scope = new InstanceScope(instance);
            return Selector<Field>.Select(scope);
        }

        public static Field Field(this object instance, string fieldName) {
            var scope = new InstanceScope(instance);
            var named = new FieldNameFilter(scope, fieldName);
            return Selector<Field>.Select(named);
        }

        public static Field Field(this object instance, Type fieldType) {
            var scope = new InstanceScope(instance);
            var named = new FieldTypeFilter(scope, fieldType);
            return Selector<Field>.Select(named);
        }

        public static Field Field(this object instance, Type fieldType, string fieldName) {
            var scope = new InstanceScope(instance);
            var typed = new FieldTypeFilter(scope, fieldType);
            var named = new FieldNameFilter(typed, fieldName);
            return Selector<Field>.Select(named);
        }

        public static Field<T> Field<T>(this object instance) {
            Field field = Field(instance, typeof(T));
            return new Field<T>(field.Info, field.Instance);
        }

        public static Field<T> Field<T>(this object instance, string fieldName) {
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
