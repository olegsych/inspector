using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    sealed class TypeMembers<TInfo, TMember> : IEnumerable<TMember>
        where TInfo : MemberInfo
        where TMember : Member<TInfo>
    {
        public TypeMembers(Type type, Func<TypeInfo, IEnumerable<TInfo>> getInfos, Func<TInfo, TMember> makeMember) {
            Type = type;
            GetInfos = getInfos;
            MakeMember = makeMember;
        }

        public Func<TypeInfo, IEnumerable<TInfo>> GetInfos { get; }
        public Func<TInfo, TMember> MakeMember { get; }
        public Type Type { get; }

        public IEnumerator<TMember> GetEnumerator() {
            Type type = Type;
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(TInfo memberInfo in GetInfos(typeInfo))
                    yield return MakeMember(memberInfo);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
