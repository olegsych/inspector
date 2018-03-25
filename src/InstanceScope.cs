using System;
using System.Collections.Generic;

namespace Inspector
{
    class InstanceScope : IScope
    {
        public InstanceScope(object instance) =>
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));

        public object Instance { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();
        IEnumerable<Constructor> IFilter<Constructor>.Get() => throw new NotImplementedException();
        IEnumerable<Event> IFilter<Event>.Get() => throw new NotImplementedException();
        IEnumerable<Field> IFilter<Field>.Get() => throw new NotImplementedException();
        IEnumerable<Method> IFilter<Method>.Get() => throw new NotImplementedException();
        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();
    }
}
