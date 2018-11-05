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

        IEnumerable<Constructor> IFilter<Constructor>.Get() =>
            new Members<ConstructorInfo, Constructor>(Type, Instance, typeInfo => typeInfo.GetConstructors, Constructor.Create, Lifetime.Instance);

        IEnumerable<Event> IFilter<Event>.Get() =>
            new Members<EventInfo, Event>(Type, Instance, typeInfo => typeInfo.GetEvents, Event.Create);

        IEnumerable<Field> IFilter<Field>.Get() =>
            new Members<FieldInfo, Field>(Type, Instance, typeInfo => typeInfo.GetFields, Field.Create);

        IEnumerable<Method> IFilter<Method>.Get() =>
            new Members<MethodInfo, Method>(Type, Instance, typeInfo => typeInfo.GetMethods, Method.Create);

        IEnumerable<Property> IFilter<Property>.Get() =>
            new Members<PropertyInfo, Property>(Type, Instance, typeInfo => typeInfo.GetProperties, Property.Create);
    }
}
