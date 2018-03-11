using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides operations common for accessing properties and fields.
    /// </summary>
    public abstract class ValueMemberInspector<TValue, TInfo> : Member<TInfo> where TInfo : MemberInfo
    {
        public abstract TValue Get();

        public abstract void Set(TValue value);

        public static implicit operator TValue(ValueMemberInspector<TValue, TInfo> member)
            => throw new NotImplementedException();

        public static bool operator ==(ValueMemberInspector<TValue, TInfo> member, TValue value)
            => throw new NotImplementedException();

        public static bool operator !=(ValueMemberInspector<TValue, TInfo> member, TValue value)
            => throw new NotImplementedException();

        public static bool operator ==(TValue value, ValueMemberInspector<TValue, TInfo> member)
            => throw new NotImplementedException();

        public static bool operator !=(TValue value, ValueMemberInspector<TValue, TInfo> member)
            => throw new NotImplementedException();
    }
}
