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

            Type instanceType = instance.GetType();
            Type fieldType = typeof(T);
            IReadOnlyList<FieldInfo> info = instanceType.GetTypeInfo()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(_ => _.FieldType == fieldType).ToList();
            if (info.Count == 0)
                throw new ArgumentException($"{instanceType} doesn't have instance fields of type {fieldType}.", nameof(T));
            if (info.Count > 1)
                throw new ArgumentException($"{instanceType} has more than one instance field of type {fieldType}.", nameof(T));

            return new Field<T>(info[0], instance);
        }
    }
}
