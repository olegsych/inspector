using System;
using System.Collections.Generic;
using System.Reflection;

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

        IEnumerable<Field> IFilter<Field>.Get() {
            Type type = Instance.GetType();
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(FieldInfo field in typeInfo.GetFields(declaredOnly))
                    yield return new Field(field, Instance);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }

        IEnumerable<Method> IFilter<Method>.Get() => throw new NotImplementedException();
        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();

        const BindingFlags declaredOnly = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
    }
}
