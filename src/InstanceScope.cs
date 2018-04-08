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

        IEnumerable<Field> IFilter<Field>.Get() =>
            Get(type => type.GetFields(declaredOnly), field => new Field(field, Instance));

        IEnumerable<Method> IFilter<Method>.Get() =>
            Get(type => type.GetMethods(declaredOnly), method => new Method(method, Instance));

        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();

        IEnumerable<T> Get<T, TMemberInfo>(Func<TypeInfo, IEnumerable<TMemberInfo>> getMemberInfos, Func<TMemberInfo, T> makeMember) {
            Type type = Instance.GetType();
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(TMemberInfo memberInfo in getMemberInfos(typeInfo))
                    yield return makeMember(memberInfo);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }

        const BindingFlags declaredOnly = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
    }
}
