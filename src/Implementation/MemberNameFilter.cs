using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class MemberNameFilter<TMember, TInfo>: Filter<TMember>
        where TMember : Member<TInfo>
        where TInfo : MemberInfo
    {
        public MemberNameFilter(IEnumerable<TMember> previous, string memberName) : base(previous) =>
            MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));

        public string MemberName { get; }

        public override IEnumerator<TMember> GetEnumerator() =>
            Previous.Where(member => member.Info.Name == MemberName).GetEnumerator();
    }
}
