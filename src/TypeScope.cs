using System;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    abstract class TypeScope : IScope
    {
        const BindingFlags declared = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        readonly BindingFlags bindingFlags;

        protected TypeScope(Type type) {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            bindingFlags = declared | BindingFlags.Static;
        }

        protected TypeScope(object instance) {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type = instance.GetType();
            bindingFlags = declared | BindingFlags.Instance;
        }

        public object Instance { get; }

        public Type Type { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();

        IEnumerable<Constructor> IFilter<Constructor>.Get() =>
            throw new NotImplementedException();

        IEnumerable<Event> IFilter<Event>.Get() =>
            throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            Get(typeInfo => typeInfo.GetFields(bindingFlags), fieldInfo => new Field(fieldInfo, Instance));

        IEnumerable<Method> IFilter<Method>.Get() =>
            Get(typeInfo => typeInfo.GetMethods(bindingFlags), methodInfo => new Method(methodInfo, Instance));

        IEnumerable<Property> IFilter<Property>.Get() =>
            throw new NotImplementedException();

        IEnumerable<TMember> Get<TMemberInfo, TMember>(Func<TypeInfo, IEnumerable<TMemberInfo>> getMemberInfos, Func<TMemberInfo, TMember> makeMember) {
            Type type = Type;
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(TMemberInfo memberInfo in getMemberInfos(typeInfo))
                    yield return makeMember(memberInfo);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }
    }
}
