using System;
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

        IEnumerable<Constructor> IScope.Constructors() => throw new NotImplementedException();
        IEnumerable<Event> IScope.Events() => throw new NotImplementedException();
        IEnumerable<Field> IScope.Fields() => throw new NotImplementedException();
        IEnumerable<Method> IScope.Methods() => throw new NotImplementedException();
        IEnumerable<Property> IScope.Properties() => throw new NotImplementedException();
    }
}
