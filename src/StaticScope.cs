using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    class StaticScope : IScope
    {
        public StaticScope(Type type) {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            TypeInfo = type.GetTypeInfo();
        }

        public TypeInfo TypeInfo { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Constructor> IEnumerable<Constructor>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Event> IEnumerable<Event>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Field> IEnumerable<Field>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Method> IEnumerable<Method>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Property> IEnumerable<Property>.GetEnumerator() => throw new NotImplementedException();
    }
}
