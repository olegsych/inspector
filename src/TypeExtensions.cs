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

        public static Field Field(this Type type) =>
            new StaticMembers(type).Field();

        public static Field Field(this Type type, Type fieldType) =>
            new StaticMembers(type).Field(fieldType);

        public static Field Field(this Type type, string fieldName) =>
            new StaticMembers(type).Field(fieldName);

        public static Field Field(this Type type, Type fieldType, string fieldName) =>
            new StaticMembers(type).Field(fieldType, fieldName);

        public static Field<T> Field<T>(this Type type) =>
            new StaticMembers(type).Field<T>();

        public static Field<T> Field<T>(this Type type, string fieldName) =>
            new StaticMembers(type).Field<T>(fieldName);

        public static IMembers Inherited(this Type type) =>
            new StaticMembers(type).InheritedFrom(type.BaseType);

        public static IMembers InheritedFrom(this Type type, Type ancestorType) =>
            new StaticMembers(type).InheritedFrom(ancestorType);

        public static IMembers InheritedFrom<T>(this Type type) =>
            new StaticMembers(type).InheritedFrom<T>();

        public static IMembers Internal(this Type type) =>
            new StaticMembers(type).Internal();

        public static Method Method(this Type type) =>
            new StaticMembers(type).Method();

        public static Method Method(this Type type, Type methodType) =>
            new StaticMembers(type).Method(methodType);

        public static Method Method(this Type type, string methodName) =>
            new StaticMembers(type).Method(methodName);

        public static Method Method(this Type type, Type methodType, string methodName) =>
            new StaticMembers(type).Method(methodType, methodName);

        public static Method<T> Method<T>(this Type type) where T : Delegate =>
            new StaticMembers(type).Method<T>();

        public static Method<T> Method<T>(this Type type, string methodName) where T : Delegate =>
            new StaticMembers(type).Method<T>(methodName);

        /// <summary>
        /// Creates an instance of given <see cref="Type"/> using the constructor that best matches given <paramref name="args"/>.
        /// </summary>
        public static object New(this Type type, params object[] args) {
            try {
                return Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, default, args, default);
            }
            catch(TargetInvocationException e) {
                throw e.InnerException;
            }
        }

        public static IMembers Private(this Type type) =>
            new StaticMembers(type).Private();

        public static Property Property(this Type type) =>
            new StaticMembers(type).Property();

        public static Property Property(this Type type, string propertyName) =>
            new StaticMembers(type).Property(propertyName);

        public static Property Property(this Type type, Type propertyType) =>
            new StaticMembers(type).Property(propertyType);

        public static Property Property(this Type type, Type propertyType, string propertyName) =>
            new StaticMembers(type).Property(propertyType, propertyName);

        public static Property<T> Property<T>(this Type type) =>
            new StaticMembers(type).Property<T>();

        public static Property<T> Property<T>(this Type type, string propertyName) =>
            new StaticMembers(type).Property<T>(propertyName);

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
