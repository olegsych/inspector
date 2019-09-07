using System;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector.Implementation
{
    abstract class TypeScope: IScope
    {
        protected TypeScope(Type type) =>
            Type = type ?? throw new ArgumentNullException(nameof(type));

        protected TypeScope(object instance) {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type = instance.GetType();
        }

        public object Instance { get; }

        public Type Type { get; }

        public IEnumerable<Constructor> Constructors() =>
            new Members<ConstructorInfo, Constructor>(Type, Instance, typeInfo => typeInfo.GetConstructors, Constructor.Create, Lifetime.Instance);

        public IEnumerable<Event> Events() =>
            new Members<EventInfo, Event>(Type, Instance, typeInfo => typeInfo.GetEvents, Event.Create);

        public IEnumerable<Field> Fields() =>
            new Members<FieldInfo, Field>(Type, Instance, typeInfo => typeInfo.GetFields, Field.Create);

        public IEnumerable<Method> Methods() =>
            new Members<MethodInfo, Method>(Type, Instance, typeInfo => typeInfo.GetMethods, Method.Create);

        public IEnumerable<Property> Properties() =>
            new Members<PropertyInfo, Property>(Type, Instance, typeInfo => typeInfo.GetProperties, Property.Create);
    }
}
