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
        readonly BindingFlags declaredMembers;

        public delegate TMember Factory(TInfo memberInfo, object instance);
        public delegate Func<BindingFlags, IEnumerable<TInfo>> InfoProvider(TypeInfo typeInfo);

        public Members(Type type, object instance, InfoProvider getMemberInfo, Factory createMember) {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            GetMemberInfo = getMemberInfo ?? throw new ArgumentNullException(nameof(getMemberInfo));
            CreateMember = createMember ?? throw new ArgumentNullException(nameof(createMember));
            Instance = instance;

            declaredMembers = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic
                | (instance == null ? BindingFlags.Static : BindingFlags.Instance);
        }

        public InfoProvider GetMemberInfo { get; }
        public Factory CreateMember { get; }
        public Type Type { get; }
        public object Instance { get; }

        public IEnumerator<TMember> GetEnumerator() {
            Type type = Type;
            do {
                TypeInfo typeInfo = type.GetTypeInfo();
                foreach(TInfo memberInfo in GetMemberInfo(typeInfo).Invoke(declaredMembers))
                    yield return CreateMember(memberInfo, Instance);
                type = typeInfo.BaseType;
            }
            while(type != null);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
