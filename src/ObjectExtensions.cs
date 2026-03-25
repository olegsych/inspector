using System;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns the only constructor declared by the instance type.
        /// </summary>
        public static Constructor Constructor(this object instance) =>
            instance.Declared().Constructor(); // Declared only because at least one constructor is always inherited from Object

        /// <summary>
        /// Returns the only constructor matching the specified delegate type.
        /// </summary>
        public static Constructor Constructor(this object instance, Type delegateType) =>
            new InstanceMembers(instance).Constructor(delegateType);

        /// <summary>
        /// Returns the only constructor with the <typeparamref name="TSignature"/> signature.
        /// </summary>
        public static Constructor<TSignature> Constructor<TSignature>(this object instance) where TSignature : Delegate =>
            new InstanceMembers(instance).Constructor<TSignature>();

        /// <summary>
        /// Returns members declared by the instance type.
        /// </summary>
        public static IMembers Declared(this object instance) =>
            new InstanceMembers(instance).DeclaredBy(instance.GetType());

        /// <summary>
        /// Returns members declared by the specified type.
        /// </summary>
        public static IMembers DeclaredBy(this object instance, Type declaringType) =>
            new InstanceMembers(instance).DeclaredBy(declaringType);

        /// <summary>
        /// Returns members declared by <typeparamref name="T"/>.
        /// </summary>
        public static IMembers DeclaredBy<T>(this object instance) =>
            new InstanceMembers(instance).DeclaredBy<T>();

        /// <summary>
        /// Returns the only event.
        /// </summary>
        public static Event Event(this object instance) =>
            new InstanceMembers(instance).Event();

        /// <summary>
        /// Returns the event with the specified name.
        /// </summary>
        public static Event Event(this object instance, string eventName) =>
            new InstanceMembers(instance).Event(eventName);

        /// <summary>
        /// Returns the only event with the specified handler type.
        /// </summary>
        public static Event Event(this object instance, Type handlerType) =>
            new InstanceMembers(instance).Event(handlerType);

        /// <summary>
        /// Returns the event with the specified handler type and name.
        /// </summary>
        public static Event Event(this object instance, Type handlerType, string eventName) =>
            new InstanceMembers(instance).Event(handlerType, eventName);

        /// <summary>
        /// Returns the only event with handler type <typeparamref name="T"/>.
        /// </summary>
        public static Event<T> Event<T>(this object instance) where T : Delegate =>
            new InstanceMembers(instance).Event<T>();

        /// <summary>
        /// Returns the event with handler type <typeparamref name="T"/> and the specified name.
        /// </summary>
        public static Event<T> Event<T>(this object instance, string eventName) where T : Delegate =>
            new InstanceMembers(instance).Event<T>(eventName);

        /// <summary>
        /// Returns the only field.
        /// </summary>
        public static Field Field(this object instance) =>
            new InstanceMembers(instance).Field();

        /// <summary>
        /// Returns the field with the specified name.
        /// </summary>
        public static Field Field(this object instance, string fieldName) =>
            new InstanceMembers(instance).Field(fieldName);

        /// <summary>
        /// Returns the only field of the specified type.
        /// </summary>
        public static Field Field(this object instance, Type fieldType) =>
            new InstanceMembers(instance).Field(fieldType);

        /// <summary>
        /// Returns the field of the specified type and name.
        /// </summary>
        public static Field Field(this object instance, Type fieldType, string fieldName) =>
            new InstanceMembers(instance).Field(fieldType, fieldName);

        /// <summary>
        /// Returns the only field of type <typeparamref name="T"/>.
        /// </summary>
        public static Field<T> Field<T>(this object instance) =>
            new InstanceMembers(instance).Field<T>();

        /// <summary>
        /// Returns the field of type <typeparamref name="T"/> with the specified name.
        /// </summary>
        public static Field<T> Field<T>(this object instance, string fieldName) =>
            new InstanceMembers(instance).Field<T>(fieldName);

        /// <summary>
        /// Returns members inherited from the base type.
        /// </summary>
        public static IMembers Inherited(this object instance) =>
            new InstanceMembers(instance).InheritedFrom(instance.GetType().BaseType);

        /// <summary>
        /// Returns members inherited from the specified type.
        /// </summary>
        public static IMembers InheritedFrom(this object instance, Type ancestorType) =>
            new InstanceMembers(instance).InheritedFrom(ancestorType);

        /// <summary>
        /// Returns members inherited from <typeparamref name="T"/>.
        /// </summary>
        public static IMembers InheritedFrom<T>(this object instance) =>
            new InstanceMembers(instance).InheritedFrom<T>();

        /// <summary>
        /// Returns members with <see langword="internal"/> accessibility.
        /// </summary>
        public static IMembers Internal(this object instance) =>
            new InstanceMembers(instance).Internal();

        /// <summary>
        /// Returns the only method declared by the instance type.
        /// </summary>
        public static Method Method(this object instance) =>
            instance.Declared().Method(); // Declared only because multiple methods are always inherited from Object

        /// <summary>
        /// Returns the method with the specified name.
        /// </summary>
        public static Method Method(this object instance, string methodName) =>
            new InstanceMembers(instance).Method(methodName);

        /// <summary>
        /// Returns the only method matching the specified delegate type.
        /// </summary>
        public static Method Method(this object instance, Type methodType) =>
            new InstanceMembers(instance).Method(methodType);

        /// <summary>
        /// Returns the method matching the specified delegate type and name.
        /// </summary>
        public static Method Method(this object instance, Type methodType, string methodName) =>
            new InstanceMembers(instance).Method(methodType, methodName);

        /// <summary>
        /// Returns the only method with the <typeparamref name="T"/> signature.
        /// </summary>
        public static Method<T> Method<T>(this object instance) where T : Delegate =>
            new InstanceMembers(instance).Method<T>();

        /// <summary>
        /// Returns the method with the <typeparamref name="T"/> signature and the specified name.
        /// </summary>
        public static Method<T> Method<T>(this object instance, string methodName) where T : Delegate =>
            new InstanceMembers(instance).Method<T>(methodName);

        /// <summary>
        /// Returns members with <see langword="private"/> accessibility.
        /// </summary>
        public static IMembers Private(this object instance) =>
            new InstanceMembers(instance).Private();

        /// <summary>
        /// Returns the only property.
        /// </summary>
        public static Property Property(this object instance) =>
            new InstanceMembers(instance).Property();

        /// <summary>
        /// Returns the property with the specified name.
        /// </summary>
        public static Property Property(this object instance, string propertyName) =>
            new InstanceMembers(instance).Property(propertyName);

        /// <summary>
        /// Returns the only property of the specified type.
        /// </summary>
        public static Property Property(this object instance, Type propertyType) =>
            new InstanceMembers(instance).Property(propertyType);

        /// <summary>
        /// Returns the property of the specified type and name.
        /// </summary>
        public static Property Property(this object instance, Type propertyType, string propertyName) =>
            new InstanceMembers(instance).Property(propertyType, propertyName);

        /// <summary>
        /// Returns the only property of type <typeparamref name="T"/>.
        /// </summary>
        public static Property<T> Property<T>(this object instance) =>
            new InstanceMembers(instance).Property<T>();

        /// <summary>
        /// Returns the property of type <typeparamref name="T"/> with the specified name.
        /// </summary>
        public static Property<T> Property<T>(this object instance, string propertyName) =>
            new InstanceMembers(instance).Property<T>(propertyName);

        /// <summary>
        /// Returns members with <see langword="protected"/> accessibility.
        /// </summary>
        public static IMembers Protected(this object instance) =>
            new InstanceMembers(instance).Protected();

        /// <summary>
        /// Returns members with <see langword="public"/> accessibility.
        /// </summary>
        public static IMembers Public(this object instance) =>
            new InstanceMembers(instance).Public();
    }
}
