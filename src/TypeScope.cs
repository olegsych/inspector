using System;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    abstract class TypeScope : IScope
    {
        readonly Lifetime lifetime;

        protected TypeScope(Type type) {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            lifetime = Lifetime.Static;
        }

        protected TypeScope(object instance) {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type = instance.GetType();
            lifetime = Lifetime.Instance;
        }

        public object Instance { get; }

        public Type Type { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();

        IEnumerable<Constructor> IFilter<Constructor>.Get() =>
            throw new NotImplementedException();

        IEnumerable<Event> IFilter<Event>.Get() =>
            throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            new Members<FieldInfo, Field>(Type, lifetime, typeInfo => typeInfo.GetFields, fieldInfo => new Field(fieldInfo, Instance));

        IEnumerable<Method> IFilter<Method>.Get() =>
            new Members<MethodInfo, Method>(Type, lifetime, typeInfo => typeInfo.GetMethods, methodInfo => new Method(methodInfo, Instance));

        IEnumerable<Property> IFilter<Property>.Get() =>
            throw new NotImplementedException();
    }
}
