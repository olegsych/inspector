using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a property of type not accessible at compile time.
    /// </summary>
    public class Property : Member<PropertyInfo>
    {
        public Property(PropertyInfo info, object instance) : base(null, null) =>
            throw new NotImplementedException();

        public override bool IsStatic =>
            throw new NotImplementedException();

        public object Value {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public object Get() =>
            throw new NotImplementedException();

        public void Set(object value) =>
            throw new NotImplementedException();
    }
}
