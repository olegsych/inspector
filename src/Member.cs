using System;
using System.Reflection;

namespace Inspector
{
    public abstract class Member<TMemberInfo> where TMemberInfo : MemberInfo
    {
        public TMemberInfo Info =>
            throw new NotImplementedException();

        public static implicit operator TMemberInfo(Member<TMemberInfo> member) =>
            throw new NotImplementedException();
    }
}
