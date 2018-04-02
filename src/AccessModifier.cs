using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// An access modifier of a type member.
    /// Matches items in <see cref="FieldAttributes.FieldAccessMask"/> and <see cref="MethodAttributes.MemberAccessMask"/>.
    /// </summary>
    enum AccessModifier
    {
        Private = 1,
        PrivateProtected = 2,
        Internal = 3,
        Protected = 4,
        ProtectedInternal = 5,
        Public = 6
    }
}
