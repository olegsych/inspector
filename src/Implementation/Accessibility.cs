using System.Reflection;

namespace Inspector.Implementation
{
    /// <summary>
    /// Accessibility of a type member.
    /// Matches items in <see cref="FieldAttributes.FieldAccessMask"/> and <see cref="MethodAttributes.MemberAccessMask"/>.
    /// </summary>
    enum Accessibility
    {
        Private = 1,
        PrivateProtected = 2,
        Internal = 3,
        Protected = 4,
        ProtectedInternal = 5,
        Public = 6
    }
}
