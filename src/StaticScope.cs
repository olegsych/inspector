using System;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    class StaticScope : IScope
    {
        public StaticScope(Type type) =>
            Type = type ?? throw new ArgumentNullException(nameof(type));

        public Type Type { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();

        IEnumerable<Constructor> IFilter<Constructor>.Get() => throw new NotImplementedException();
        IEnumerable<Event> IFilter<Event>.Get() => throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() {
            Type type = Type;
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(FieldInfo fieldInfo in typeInfo.GetFields(declaredOnly))
                    yield return new Field(fieldInfo);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }

        IEnumerable<Method> IFilter<Method>.Get() => throw new NotImplementedException();
        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();

        const BindingFlags declaredOnly = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
    }
}
