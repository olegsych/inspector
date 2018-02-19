﻿using System;
using System.Reflection;

namespace Inspector
{
    public class Field<T>
    {
        readonly FieldInfo info;
        readonly object instance;

        public Field(FieldInfo info, object instance)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (info.FieldType != typeof(T))
                throw new ArgumentException($"Field type {info.FieldType} doesn't match expected {typeof(T)}.", nameof(info));
            this.info = info;

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (!info.DeclaringType.GetTypeInfo().IsInstanceOfType(instance))
                throw new ArgumentException($"Instance type {instance.GetType()} doesn't match type {info.DeclaringType} where field is declared.", nameof(instance));
            this.instance = instance;
        }

        public T Value
        {
            get => (T)info.GetValue(instance);
            set => info.SetValue(instance, value);
        }
    }
}
