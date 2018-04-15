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

        public delegate TMember Factory(TInfo memberInfo, object instance);
        public delegate Func<BindingFlags, IEnumerable<TInfo>> InfoProvider(TypeInfo typeInfo);

        public InfoProvider GetMemberInfo { get; }
        public Factory CreateMember { get; }
        public Type Type { get; }
        public object Instance { get; }
        readonly BindingFlags bindingFlags;

        public Members(Type type, object instance, InfoProvider getMemberInfo, Factory createMember) {
            Type = type;
            Instance = instance;
            GetMemberInfo = getMemberInfo;
            CreateMember = createMember;
            bindingFlags = declared | (instance == null ? BindingFlags.Static : BindingFlags.Instance);
        }

        public IEnumerator<TMember> GetEnumerator() {
            Type type = Type;
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(TInfo memberInfo in GetMemberInfo(typeInfo).Invoke(bindingFlags))
                    yield return CreateMember(memberInfo, Instance);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
