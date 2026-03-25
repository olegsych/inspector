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
        /// <summary>
        /// Returns the only static constructor.
        /// </summary>
        public static Constructor Constructor(this Type type) =>
            new StaticMembers(type).Constructor();

        /// <summary>
        /// Returns static members declared by the type.
        /// </summary>
        public static IMembers Declared(this Type type) =>
            new StaticMembers(type).DeclaredBy(type);

        /// <summary>
        /// Returns static members declared by the specified type.
        /// </summary>
        public static IMembers DeclaredBy(this Type type, Type declaringType) =>
            new StaticMembers(type).DeclaredBy(declaringType);

        /// <summary>
        /// Returns static members declared by <typeparamref name="T"/>.
        /// </summary>
        public static IMembers DeclaredBy<T>(this Type type) =>
            new StaticMembers(type).DeclaredBy<T>();

        /// <summary>
        /// Returns the only static event.
        /// </summary>
        public static Event Event(this Type type) =>
            new StaticMembers(type).Event();

        /// <summary>
        /// Returns the static event with the specified name.
        /// </summary>
        public static Event Event(this Type type, string eventName) =>
            new StaticMembers(type).Event(eventName);

        /// <summary>
        /// Returns the only static event with the specified handler type.
        /// </summary>
        public static Event Event(this Type type, Type handlerType) =>
            new StaticMembers(type).Event(handlerType);

        /// <summary>
        /// Returns the static event with the specified handler type and name.
        /// </summary>
        public static Event Event(this Type type, Type handlerType, string eventName) =>
            new StaticMembers(type).Event(handlerType, eventName);

        /// <summary>
        /// Returns the only static event with handler type <typeparamref name="T"/>.
        /// </summary>
        public static Event<T> Event<T>(this Type type) where T : Delegate =>
            new StaticMembers(type).Event<T>();

        /// <summary>
        /// Returns the static event with handler type <typeparamref name="T"/> and the specified name.
        /// </summary>
        public static Event<T> Event<T>(this Type type, string eventName) where T : Delegate =>
            new StaticMembers(type).Event<T>(eventName);

        /// <summary>
        /// Returns the only static field.
        /// </summary>
        public static Field Field(this Type type) =>
            new StaticMembers(type).Field();

        /// <summary>
        /// Returns the only static field of the specified type.
        /// </summary>
        public static Field Field(this Type type, Type fieldType) =>
            new StaticMembers(type).Field(fieldType);

        /// <summary>
        /// Returns the static field with the specified name.
        /// </summary>
        public static Field Field(this Type type, string fieldName) =>
            new StaticMembers(type).Field(fieldName);

        /// <summary>
        /// Returns the static field of the specified type and name.
        /// </summary>
        public static Field Field(this Type type, Type fieldType, string fieldName) =>
            new StaticMembers(type).Field(fieldType, fieldName);

        /// <summary>
        /// Returns the only static field of type <typeparamref name="T"/>.
        /// </summary>
        public static Field<T> Field<T>(this Type type) =>
            new StaticMembers(type).Field<T>();

        /// <summary>
        /// Returns the static field of type <typeparamref name="T"/> with the specified name.
        /// </summary>
        public static Field<T> Field<T>(this Type type, string fieldName) =>
            new StaticMembers(type).Field<T>(fieldName);

        /// <summary>
        /// Returns static members inherited from the base type.
        /// </summary>
        public static IMembers Inherited(this Type type) =>
            new StaticMembers(type).InheritedFrom(type.BaseType);

        /// <summary>
        /// Returns static members inherited from the specified type.
        /// </summary>
        public static IMembers InheritedFrom(this Type type, Type ancestorType) =>
            new StaticMembers(type).InheritedFrom(ancestorType);

        /// <summary>
        /// Returns static members inherited from <typeparamref name="T"/>.
        /// </summary>
        public static IMembers InheritedFrom<T>(this Type type) =>
            new StaticMembers(type).InheritedFrom<T>();

        /// <summary>
        /// Returns static members with <see langword="internal"/> accessibility.
        /// </summary>
        public static IMembers Internal(this Type type) =>
            new StaticMembers(type).Internal();

        /// <summary>
        /// Returns the only static method.
        /// </summary>
        public static Method Method(this Type type) =>
            new StaticMembers(type).Method();

        /// <summary>
        /// Returns the only static method matching the specified delegate type.
        /// </summary>
        public static Method Method(this Type type, Type methodType) =>
            new StaticMembers(type).Method(methodType);

        /// <summary>
        /// Returns the static method with the specified name.
        /// </summary>
        public static Method Method(this Type type, string methodName) =>
            new StaticMembers(type).Method(methodName);

        /// <summary>
        /// Returns the static method matching the specified delegate type and name.
        /// </summary>
        public static Method Method(this Type type, Type methodType, string methodName) =>
            new StaticMembers(type).Method(methodType, methodName);

        /// <summary>
        /// Returns the only static method with the <typeparamref name="T"/> signature.
        /// </summary>
        public static Method<T> Method<T>(this Type type) where T : Delegate =>
            new StaticMembers(type).Method<T>();

        /// <summary>
        /// Returns the static method with the <typeparamref name="T"/> signature and the specified name.
        /// </summary>
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

        /// <summary>
        /// Returns static members with <see langword="private"/> accessibility.
        /// </summary>
        public static IMembers Private(this Type type) =>
            new StaticMembers(type).Private();

        /// <summary>
        /// Returns the only static property.
        /// </summary>
        public static Property Property(this Type type) =>
            new StaticMembers(type).Property();

        /// <summary>
        /// Returns the static property with the specified name.
        /// </summary>
        public static Property Property(this Type type, string propertyName) =>
            new StaticMembers(type).Property(propertyName);

        /// <summary>
        /// Returns the only static property of the specified type.
        /// </summary>
        public static Property Property(this Type type, Type propertyType) =>
            new StaticMembers(type).Property(propertyType);

        /// <summary>
        /// Returns the static property of the specified type and name.
        /// </summary>
        public static Property Property(this Type type, Type propertyType, string propertyName) =>
            new StaticMembers(type).Property(propertyType, propertyName);

        /// <summary>
        /// Returns the only static property of type <typeparamref name="T"/>.
        /// </summary>
        public static Property<T> Property<T>(this Type type) =>
            new StaticMembers(type).Property<T>();

        /// <summary>
        /// Returns the static property of type <typeparamref name="T"/> with the specified name.
        /// </summary>
        public static Property<T> Property<T>(this Type type, string propertyName) =>
            new StaticMembers(type).Property<T>(propertyName);

        /// <summary>
        /// Returns static members with <see langword="protected"/> accessibility.
        /// </summary>
        public static IMembers Protected(this Type type) =>
            new StaticMembers(type).Protected();

        /// <summary>
        /// Returns static members with <see langword="public"/> accessibility.
        /// </summary>
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
