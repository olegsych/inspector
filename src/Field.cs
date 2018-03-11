using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a field of type not accessible at compile time.
    /// </summary>
    public class Field : ValueMember<FieldInfo>
    {
        public override object Get() => throw new NotImplementedException();

        public override void Set(object value) => throw new NotImplementedException();
    }

    /// <summary>
    /// Provides access to a field of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    public class Field<T> : ValueMember<T, FieldInfo>
    {
        readonly FieldInfo info;
        readonly object instance;

        public Field(FieldInfo info, object instance) {
            if(info == null)
                throw new ArgumentNullException(nameof(info));
            if(info.FieldType != typeof(T))
                throw new ArgumentException($"Field type {info.FieldType} doesn't match expected {typeof(T)}.", nameof(info));
            this.info = info;

            if(instance == null)
                throw new ArgumentNullException(nameof(instance));
            if(!info.DeclaringType.GetTypeInfo().IsInstanceOfType(instance))
                throw new ArgumentException($"Instance type {instance.GetType()} doesn't match type {info.DeclaringType} where field is declared.", nameof(instance));
            this.instance = instance;
        }

        public override T Get() => throw new NotImplementedException();

        public override void Set(T value) => throw new NotImplementedException();
    }
}
