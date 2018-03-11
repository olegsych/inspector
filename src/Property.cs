using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a property of type not accessible at compile time.
    /// </summary>
    public class Property : ValueMember<PropertyInfo>
    {
        public override object Get() => throw new NotImplementedException();

        public override void Set(object value) => throw new NotImplementedException();
    }

    /// <summary>
    /// Provides access to a property of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Property type</typeparam>
    public class Property<T> : ValueMember<T, PropertyInfo>
    {
        public override T Get() => throw new NotImplementedException();

        public override void Set(T value) => throw new NotImplementedException();
    }
}
