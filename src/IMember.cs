using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Represents a type member with access to its <see cref="MemberInfo"/> metadata.
    /// </summary>
    /// <typeparam name="TMemberInfo">
    /// A class derived from <see cref="MemberInfo"/> describing the type member.
    /// </typeparam>
    public interface IMember<out TMemberInfo> where TMemberInfo : MemberInfo
    {
        /// <summary>
        /// Gets the member metadata.
        /// </summary>
        TMemberInfo Info { get; }
    }
}
