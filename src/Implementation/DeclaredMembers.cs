using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class DeclaredMembers<TMember, TInfo>: Filter<TMember>
        where TMember: Member<TInfo>
        where TInfo: MemberInfo
    {
        public DeclaredMembers(IEnumerable<TMember> source, Type declaringType): base(source) =>
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));

        public Type DeclaringType { get; }

        public override IEnumerator<TMember> GetEnumerator() =>
            Source.Where(p => p.Info.DeclaringType == DeclaringType).GetEnumerator();
    }
}
