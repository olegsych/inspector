using System;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    abstract class TypeScope : IScope
    {
        protected TypeScope(Type type) =>
            Type = type ?? throw new ArgumentNullException(nameof(type));

        protected TypeScope(object instance) {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type = instance.GetType();
        }

        public object Instance { get; }

        public Type Type { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();

        IEnumerable<Constructor> IFilter<Constructor>.Get() =>
            throw new NotImplementedException();

        IEnumerable<Event> IFilter<Event>.Get() =>
            throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            new Members<FieldInfo, Field>(Type, Instance, typeInfo => typeInfo.GetFields, Field.Create);

        IEnumerable<Method> IFilter<Method>.Get() =>
            new Members<MethodInfo, Method>(Type, Instance, typeInfo => typeInfo.GetMethods, Method.Create);

        IEnumerable<Property> IFilter<Property>.Get() =>
            throw new NotImplementedException();
    }
}
