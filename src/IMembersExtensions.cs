using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for <see cref="IMembers"/>.
    /// </summary>
    public static class IMembersExtensions
    {
        public static IMembers Internal(this IMembers members) =>
            new AccessibleMembers(members, Accessibility.Internal);

        public static IMembers Private(this IMembers members) =>
            new AccessibleMembers(members, Accessibility.Private);

        public static IMembers Protected(this IMembers members) =>
            new AccessibleMembers(members, Accessibility.Protected);

        public static IMembers Public(this IMembers members) =>
            new AccessibleMembers(members, Accessibility.Public);
    }
}
