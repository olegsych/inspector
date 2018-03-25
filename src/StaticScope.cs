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

        string IDescriptor.Describe() => throw new NotImplementedException();

        IEnumerable<Constructor> IFilter<Constructor>.Get() => throw new NotImplementedException();
        IEnumerable<Event> IFilter<Event>.Get() => throw new NotImplementedException();
        IEnumerable<Field> IFilter<Field>.Get() => throw new NotImplementedException();
        IEnumerable<Method> IFilter<Method>.Get() => throw new NotImplementedException();
        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();
    }
}
