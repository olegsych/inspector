using System;
using System.Reflection;

namespace Inspector
{
    public abstract class MemberInspector<TMemberInfo> where TMemberInfo : MemberInfo
    {
        public TMemberInfo Info => throw new NotImplementedException();

        public static implicit operator TMemberInfo(MemberInspector<TMemberInfo> member)
            => throw new NotImplementedException();
    }
}
