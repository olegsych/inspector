using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Filters members by name.
    /// </summary>
    sealed class MemberNameFilter<TMember, TInfo> : IFilter<TMember>, IDecorator<IFilter<TMember>>
        where TMember : Member<TInfo>
        where TInfo : MemberInfo
    {
        public MemberNameFilter(IFilter<TMember> previous, string memberName) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
        }

        public IFilter<TMember> Previous { get; }

        public string MemberName { get; }

        IEnumerable<TMember> IFilter<TMember>.Get() =>
            Previous.Get().Where(member => member.Info.Name == MemberName);
    }
}
