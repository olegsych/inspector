using System;
using System.Collections;
using System.Collections.Generic;

namespace Inspector
{
    class InstanceScope : IScope
    {
        public InstanceScope(object instance) =>
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));

        public object Instance { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Constructor> IEnumerable<Constructor>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Event> IEnumerable<Event>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Field> IEnumerable<Field>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Method> IEnumerable<Method>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Property> IEnumerable<Property>.GetEnumerator() => throw new NotImplementedException();
    }
}
