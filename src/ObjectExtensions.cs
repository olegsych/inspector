using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    public static class ObjectExtensions
    {
        public static Field<T> Field<T>(this object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            TypeInfo instanceType = instance.GetType().GetTypeInfo();

            if (IsCastleDynamicProxy(instanceType))
                instanceType = instanceType.BaseType.GetTypeInfo();

            Type fieldType = typeof(T);
            IReadOnlyList<FieldInfo> info = instanceType
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(_ => _.FieldType == fieldType).ToList();
            if (info.Count == 0)
                throw new ArgumentException($"{instanceType} doesn't have instance fields of type {fieldType}.", nameof(T));
            if (info.Count > 1)
                throw new ArgumentException($"{instanceType} has more than one instance field of type {fieldType}.", nameof(T));

            return new Field<T>(info[0], instance);
        }

        static bool IsCastleDynamicProxy(TypeInfo instanceType)
        {
            return instanceType.Assembly.GetName().Name == "DynamicProxyGenAssembly2";
        }
    }
}
