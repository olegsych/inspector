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
        const BindingFlags declared = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        readonly Func<TypeInfo, Func<BindingFlags, IEnumerable<TInfo>>> getMemberInfos;
        readonly Func<TInfo, object, TMember> createMember;
        readonly Type type;
        readonly object instance;
        readonly BindingFlags bindingFlags;

        public Members(Type type, object instance, Func<TypeInfo, Func<BindingFlags, IEnumerable<TInfo>>> getMemberInfos, Func<TInfo, object, TMember> createMember) {
            this.type = type;
            this.instance = instance;
            this.getMemberInfos = getMemberInfos;
            this.createMember = createMember;
            bindingFlags = declared | (instance == null ? BindingFlags.Static : BindingFlags.Instance);
        }

        public IEnumerator<TMember> GetEnumerator() {
            Type type = this.type;
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(TInfo memberInfo in getMemberInfos(typeInfo).Invoke(bindingFlags))
                    yield return createMember(memberInfo, instance);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
