using System;
using System.Collections.Generic;

namespace Inspector
{
    class StaticScope : IScope
    {
        public StaticScope(Type type) => throw new NotImplementedException();

        IEnumerable<Constructor> IScope.Constructors() => throw new NotImplementedException();
        IEnumerable<Event> IScope.Events() => throw new NotImplementedException();
        IEnumerable<Field> IScope.Fields() => throw new NotImplementedException();
        IEnumerable<Method> IScope.Methods() => throw new NotImplementedException();
        IEnumerable<Property> IScope.Properties() => throw new NotImplementedException();
    }
}
