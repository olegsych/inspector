using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    sealed class Members<TInfo, TMember> : IEnumerable<TMember>
        where TInfo : MemberInfo
        where TMember : Member<TInfo>
    {
        readonly BindingFlags bindingFlags;
        readonly Func<TypeInfo, Func<BindingFlags, IEnumerable<TInfo>>> getMemberInfos;
        readonly Func<TInfo, TMember> makeMember;
        readonly Type type;

        public Members(Type type, Lifetime lifetime, Func<TypeInfo, Func<BindingFlags, IEnumerable<TInfo>>> getMemberInfos, Func<TInfo, TMember> makeMember) {
            bindingFlags = (BindingFlags)lifetime | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            this.type = type;
            this.getMemberInfos = getMemberInfos;
            this.makeMember = makeMember;
        }

        public IEnumerator<TMember> GetEnumerator() {
            Type type = this.type;
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(TInfo memberInfo in getMemberInfos(typeInfo).Invoke(bindingFlags))
                    yield return makeMember(memberInfo);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
