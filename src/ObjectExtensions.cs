using System;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        public static IMembers Declared(this object instance) =>
            new InstanceMembers(instance).DeclaredBy(instance.GetType());

        public static IMembers DeclaredBy(this object instance, Type declaringType) =>
            new InstanceMembers(instance).DeclaredBy(declaringType);

        public static IMembers DeclaredBy<T>(this object instance) =>
            new InstanceMembers(instance).DeclaredBy<T>();

        public static IMembers Internal(this object instance) =>
            new InstanceMembers(instance).Internal();

        public static IMembers Private(this object instance) =>
            new InstanceMembers(instance).Private();

        public static IMembers Protected(this object instance) =>
            new InstanceMembers(instance).Protected();

        public static IMembers Public(this object instance) =>
            new InstanceMembers(instance).Public();
    }
}
