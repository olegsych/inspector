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
        public static IMembers DeclaredBy(this IMembers members, Type declaringType) =>
            new DeclaredMembers(members, declaringType);

        public static IMembers DeclaredBy<T>(this IMembers members) =>
            new DeclaredMembers(members, typeof(T));

        public static IMembers InheritedFrom(this IMembers members, Type ancestorType) =>
            new InheritedMembers(members, ancestorType);

        public static IMembers InheritedFrom<T>(this IMembers members) =>
            new InheritedMembers(members, typeof(T));

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
