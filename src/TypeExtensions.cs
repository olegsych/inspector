using System;
using System.Reflection;
using System.Runtime.Serialization;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        public static Constructor Constructor(this Type type) =>
            new StaticMembers(type).Constructor();

        public static IMembers Declared(this Type type) =>
            new StaticMembers(type).DeclaredBy(type);

        public static IMembers DeclaredBy(this Type type, Type declaringType) =>
            new StaticMembers(type).DeclaredBy(declaringType);

        public static IMembers DeclaredBy<T>(this Type type) =>
            new StaticMembers(type).DeclaredBy<T>();

        public static Event Event(this Type type) =>
            new StaticMembers(type).Event();

        public static Event Event(this Type type, string eventName) =>
            new StaticMembers(type).Event(eventName);

        public static Event Event(this Type type, Type handlerType) =>
            new StaticMembers(type).Event(handlerType);

        public static Event Event(this Type type, Type handlerType, string eventName) =>
            new StaticMembers(type).Event(handlerType, eventName);

        public static Event<T> Event<T>(this Type type) where T : Delegate =>
            new StaticMembers(type).Event<T>();

        public static Event<T> Event<T>(this Type type, string eventName) where T : Delegate =>
            new StaticMembers(type).Event<T>(eventName);

        public static IMembers Internal(this Type type) =>
            new StaticMembers(type).Internal();

        /// <summary>
        /// Creates an instance of given <see cref="Type"/> using the constructor that best matches given <paramref name="args"/>.
        /// </summary>
        public static object New(this Type type, params object[] args) =>
            Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, default, args, default);

        public static IMembers Private(this Type type) =>
            new StaticMembers(type).Private();

        public static IMembers Protected(this Type type) =>
            new StaticMembers(type).Protected();

        public static IMembers Public(this Type type) =>
            new StaticMembers(type).Public();

        /// <summary>
        /// Creates an uninitialized instance of given <see cref="Type"/>.
        /// </summary>
        /// <returns>
        /// A zeroed object of given type, created without invoking any instance constructors.
        /// </returns>
        public static object Uninitialized(this Type type) =>
            FormatterServices.GetUninitializedObject(type);
    }
}
