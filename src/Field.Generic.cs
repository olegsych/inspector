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
        public Field(FieldInfo info, object instance) : base(null, null) =>
            throw new NotImplementedException();

        public new T Value {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public new T Get() =>
            throw new NotImplementedException();

        public void Set(T value) =>
            throw new NotImplementedException();

        public static implicit operator T(Field<T> field) =>
            throw new NotImplementedException();
    }

}
