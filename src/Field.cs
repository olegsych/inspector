using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a field of type not accessible at compile time.
    /// </summary>
    public class Field : Member<FieldInfo>
    {
        public Field(FieldInfo info, object instance) : base(null, null) =>
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
