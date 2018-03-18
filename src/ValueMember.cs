using System;
using System.Reflection;

namespace Inspector
{
    public abstract class ValueMember<TInfo> : Member<TInfo> where TInfo : MemberInfo
    {
        public object Value {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public abstract object Get();

        public abstract void Set(object value);
    }

    /// <summary>
    /// Provides operations common for accessing properties and fields.
    /// </summary>
    public abstract class ValueMember<TValue, TInfo> : Member<TInfo> where TInfo : MemberInfo
    {
        public TValue Value {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public abstract TValue Get();

        public abstract void Set(TValue value);

        public static implicit operator TValue(ValueMember<TValue, TInfo> member) =>
            throw new NotImplementedException();

        public static bool operator ==(ValueMember<TValue, TInfo> member, TValue value) =>
            throw new NotImplementedException();

        public static bool operator !=(ValueMember<TValue, TInfo> member, TValue value) =>
            throw new NotImplementedException();

        public static bool operator ==(TValue value, ValueMember<TValue, TInfo> member) =>
            throw new NotImplementedException();

        public static bool operator !=(TValue value, ValueMember<TValue, TInfo> member) =>
            throw new NotImplementedException();
    }
}
