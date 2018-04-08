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

        IEnumerable<Field> IFilter<Field>.Get() =>
            Get(typeInfo => typeInfo.GetFields(declaredOnly), fieldInfo => new Field(fieldInfo));

        IEnumerable<Method> IFilter<Method>.Get() =>
            Get(typeInfo => typeInfo.GetMethods(declaredOnly), methodInfo => new Method(methodInfo));

        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();

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

        const BindingFlags declaredOnly = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
    }
}
