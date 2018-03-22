using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a field of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of field value.</typeparam>
    public class Field<T> : Field
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Field{T}"/> class.
        /// </summary>
        public Field(FieldInfo info, object instance) : base(info, instance) {
            if(typeof(T) != info.FieldType)
                throw new ArgumentException($"Field type {info.FieldType.FullName} doesn't match expected {typeof(T).FullName}.", nameof(info));
        }

        /// <summary>
        /// Gets or sets the field value.
        /// </summary>
        public new T Value {
            get => Get();
            set => Set(value);
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        public new T Get() =>
            (T)base.Get();

        /// <summary>
        /// Sets the field value.
        /// </summary>
        public void Set(T value) =>
            base.Set(value);

        /// <summary>
        /// Implicitly converts the field to it's value for convenient use in assertions.
        /// </summary>
        public static implicit operator T(Field<T> field) =>
            field != null ? field.Get() : default;
    }

}
