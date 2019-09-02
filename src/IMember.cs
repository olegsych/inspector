using System.Reflection;

namespace Inspector
{
    public interface IMember<out TMemberInfo> where TMemberInfo : MemberInfo
    {
        TMemberInfo Info { get; }
    }
}
