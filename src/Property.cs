using System;
using System.Reflection;

namespace Inspector
{
    public class Property
    {

    }

    /// <summary>
    /// Provides access to a property.
    /// </summary>
    public class Property<T> : ValueMember<T, PropertyInfo>
    {
        public override T Get() => throw new NotImplementedException();

        public override void Set(T value) => throw new NotImplementedException();
    }
}
