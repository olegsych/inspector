using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a property of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Property type</typeparam>
    public class Property<T> : Property
    {
        public Property(PropertyInfo info, object instance) : base(null, null) =>
            throw new NotImplementedException();

        public new T Value {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public new T Get() =>
            throw new NotImplementedException();

        public void Set(T value) =>
            throw new NotImplementedException();

        public static implicit operator T(Property<T> field) =>
            throw new NotImplementedException();
    }
}
