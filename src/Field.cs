using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a field of type not accessible at compile time.
    /// </summary>
    public class Field : ValueMember<FieldInfo>
    {
        protected Field(FieldInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public override object Get() => throw new NotImplementedException();

        public override void Set(object value) => throw new NotImplementedException();
    }

    /// <summary>
    /// Provides access to a field of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    public class Field<T> : ValueMember<T, FieldInfo>
    {
        public Field(FieldInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public override T Get() => throw new NotImplementedException();

        public override void Set(T value) => throw new NotImplementedException();
    }
}
