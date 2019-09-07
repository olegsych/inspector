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
        public DeclaredMembers(IEnumerable<TMember> previous, Type declaringType): base(previous) =>
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));

        public Type DeclaringType { get; }

        public override IEnumerator<TMember> GetEnumerator() =>
            Previous.Where(p => p.Info.DeclaringType == DeclaringType).GetEnumerator();
    }
}
