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
        static readonly IDelegateFactory<ConstructorInfo> constructorFactory = new ConstructorDelegateFactory();
        static readonly IDelegateFactory<MethodInfo> methodFactory = new MethodDelegateFactory();

        /// <summary>
        /// Returns the only constructor.
        /// </summary>
        public static Constructor Constructor(this IMembers members) =>
            members.Constructors().Single();

        /// <summary>
        /// Returns the only constructor matching the specified delegate type.
        /// </summary>
        public static Constructor Constructor(this IMembers members, Type delegateType) =>
            new ConstructorTypeFilter(members.Constructors(), delegateType, constructorFactory).Single();

        /// <summary>
        /// Returns the only constructor with the <typeparamref name="TSignature"/> signature.
        /// </summary>
        public static Constructor<TSignature> Constructor<TSignature>(this IMembers members) where TSignature : Delegate =>
            new Constructor<TSignature>(members.Constructor(typeof(TSignature)), constructorFactory);

        /// <summary>
        /// Returns members declared by the specified type.
        /// </summary>
        public static IMembers DeclaredBy(this IMembers members, Type declaringType) =>
            new DeclaredMembers(members, declaringType);

        /// <summary>
        /// Returns members declared by <typeparamref name="T"/>.
        /// </summary>
        public static IMembers DeclaredBy<T>(this IMembers members) =>
            new DeclaredMembers(members, typeof(T));

        /// <summary>
        /// Returns the only event.
        /// </summary>
        public static Event Event(this IMembers members) =>
            members.Events().Single();

        /// <summary>
        /// Returns the event with the specified name.
        /// </summary>
        public static Event Event(this IMembers members, string eventName) =>
            new MemberNameFilter<Event, EventInfo>(members.Events(), eventName).Single();

        /// <summary>
        /// Returns the only event with the specified handler type.
        /// </summary>
        public static Event Event(this IMembers members, Type handlerType) =>
            new EventTypeFilter(members.Events(), handlerType).Single();

        /// <summary>
        /// Returns the event with the specified handler type and name.
        /// </summary>
        public static Event Event(this IMembers members, Type handlerType, string eventName) {
            var typed = new EventTypeFilter(members.Events(), handlerType);
            var named = new MemberNameFilter<Event, EventInfo>(typed, eventName);
            return named.Single();
        }

        /// <summary>
        /// Returns the only event with handler type <typeparamref name="T"/>.
        /// </summary>
        public static Event<T> Event<T>(this IMembers members) where T : Delegate =>
            new Event<T>(members.Event(typeof(T)));

        /// <summary>
        /// Returns the event with handler type <typeparamref name="T"/> and the specified name.
        /// </summary>
        public static Event<T> Event<T>(this IMembers members, string eventName) where T : Delegate =>
            new Event<T>(members.Event(typeof(T), eventName));

        /// <summary>
        /// Returns the only field.
        /// </summary>
        public static Field Field(this IMembers members) =>
            members.Fields().Single();

        /// <summary>
        /// Returns the field with the specified name.
        /// </summary>
        public static Field Field(this IMembers members, string fieldName) =>
            new MemberNameFilter<Field, FieldInfo>(members.Fields(), fieldName).Single();

        /// <summary>
        /// Returns the only field of the specified type.
        /// </summary>
        public static Field Field(this IMembers members, Type fieldType) =>
            new FieldTypeFilter(members.Fields(), fieldType).Single();

        /// <summary>
        /// Returns the field of the specified type and name.
        /// </summary>
        public static Field Field(this IMembers members, Type fieldType, string fieldName) {
            var typed = new FieldTypeFilter(members.Fields(), fieldType);
            var named = new MemberNameFilter<Field, FieldInfo>(typed, fieldName);
            return named.Single();
        }

        /// <summary>
        /// Returns the only field of type <typeparamref name="T"/>.
        /// </summary>
        public static Field<T> Field<T>(this IMembers members) =>
            new Field<T>(members.Field(typeof(T)));

        /// <summary>
        /// Returns the field of type <typeparamref name="T"/> with the specified name.
        /// </summary>
        public static Field<T> Field<T>(this IMembers members, string fieldName) =>
            new Field<T>(members.Field(typeof(T), fieldName));

        /// <summary>
        /// Returns members inherited from the specified type.
        /// </summary>
        public static IMembers InheritedFrom(this IMembers members, Type ancestorType) =>
            new InheritedMembers(members, ancestorType);

        /// <summary>
        /// Returns members inherited from <typeparamref name="T"/>.
        /// </summary>
        public static IMembers InheritedFrom<T>(this IMembers members) =>
            new InheritedMembers(members, typeof(T));

        /// <summary>
        /// Returns members with <see langword="internal"/> accessibility.
        /// </summary>
        public static IMembers Internal(this IMembers members) =>
            new AccessibleMembers(members, Accessibility.Internal);

        /// <summary>
        /// Returns the only method.
        /// </summary>
        public static Method Method(this IMembers members) =>
            members.Methods().Single();

        /// <summary>
        /// Returns the method with the specified name.
        /// </summary>
        public static Method Method(this IMembers members, string methodName) =>
            new MemberNameFilter<Method, MethodInfo>(members.Methods(), methodName).Single();

        /// <summary>
        /// Returns the only method matching the specified delegate type.
        /// </summary>
        public static Method Method(this IMembers members, Type methodType) =>
            new MethodTypeFilter(members.Methods(), methodType, methodFactory).Single();

        /// <summary>
        /// Returns the method matching the specified delegate type and name.
        /// </summary>
        public static Method Method(this IMembers members, Type methodType, string methodName) {
            var typed = new MethodTypeFilter(members.Methods(), methodType, methodFactory);
            var named = new MemberNameFilter<Method, MethodInfo>(typed, methodName);
            return named.Single();
        }

        /// <summary>
        /// Returns the only method with the <typeparamref name="T"/> signature.
        /// </summary>
        public static Method<T> Method<T>(this IMembers members) where T : Delegate =>
            new Method<T>(members.Method(typeof(T)), methodFactory);

        /// <summary>
        /// Returns the method with the <typeparamref name="T"/> signature and the specified name.
        /// </summary>
        public static Method<T> Method<T>(this IMembers members, string methodName) where T : Delegate =>
            new Method<T>(members.Method(typeof(T), methodName), methodFactory);

        /// <summary>
        /// Returns members with <see langword="private"/> accessibility.
        /// </summary>
        public static IMembers Private(this IMembers members) =>
            new AccessibleMembers(members, Accessibility.Private);

        /// <summary>
        /// Returns the only property.
        /// </summary>
        public static Property Property(this IMembers members) =>
            members.Properties().Single();

        /// <summary>
        /// Returns the property with the specified name.
        /// </summary>
        public static Property Property(this IMembers members, string propertyName) =>
            new MemberNameFilter<Property, PropertyInfo>(members.Properties(), propertyName).Single();

        /// <summary>
        /// Returns the only property of the specified type.
        /// </summary>
        public static Property Property(this IMembers members, Type propertyType) =>
            new PropertyTypeFilter(members.Properties(), propertyType).Single();

        /// <summary>
        /// Returns the property of the specified type and name.
        /// </summary>
        public static Property Property(this IMembers members, Type propertyType, string propertyName) {
            var typed = new PropertyTypeFilter(members.Properties(), propertyType);
            var named = new MemberNameFilter<Property, PropertyInfo>(typed, propertyName);
            return named.Single();
        }

        /// <summary>
        /// Returns the only property of type <typeparamref name="T"/>.
        /// </summary>
        public static Property<T> Property<T>(this IMembers members) =>
            new Property<T>(members.Property(typeof(T)));

        /// <summary>
        /// Returns the property of type <typeparamref name="T"/> with the specified name.
        /// </summary>
        public static Property<T> Property<T>(this IMembers members, string propertyName) =>
            new Property<T>(members.Property(typeof(T), propertyName));

        /// <summary>
        /// Returns members with <see langword="protected"/> accessibility.
        /// </summary>
        public static IMembers Protected(this IMembers members) =>
            new AccessibleMembers(members, Accessibility.Protected);

        /// <summary>
        /// Returns members with <see langword="public"/> accessibility.
        /// </summary>
        public static IMembers Public(this IMembers members) =>
            new AccessibleMembers(members, Accessibility.Public);
    }
}
