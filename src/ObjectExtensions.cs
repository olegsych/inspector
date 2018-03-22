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

        public static Field<T> Field<T>(this object instance) {
            if(instance == null)
                throw new ArgumentNullException(nameof(instance));

            TypeInfo instanceType = instance.GetType().GetTypeInfo();

            if(IsCastleDynamicProxy(instanceType))
                instanceType = instanceType.BaseType.GetTypeInfo();

            Type fieldType = typeof(T);
            IReadOnlyList<FieldInfo> info = instanceType
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(_ => _.FieldType == fieldType).ToList();
            if(info.Count == 0)
                throw new ArgumentException($"{instanceType} doesn't have instance fields of type {fieldType}.", nameof(T));
            if(info.Count > 1)
                throw new ArgumentException($"{instanceType} has more than one instance field of type {fieldType}.", nameof(T));

            return new Field<T>(info[0], instance);
        }

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
