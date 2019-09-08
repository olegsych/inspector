using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class DeclarationFilter<TMember, TInfo>: Filter<TMember>
        where TMember: Member<TInfo>
        where TInfo: MemberInfo
    {
        public DeclarationFilter(IEnumerable<TMember> source, Type declaringType): base(source) =>
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));

        public Type DeclaringType { get; }

        protected override IEnumerable<TMember> Where() =>
            Source.Where(p => p.Info.DeclaringType == DeclaringType);
    }
}
