using System;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        public static Constructor Constructor(this object instance) =>
            instance.Declared().Constructor(); // Declared only because at least one constructor is always inherited from Object

        public static Constructor Constructor(this object instance, Type delegateType) =>
            new InstanceMembers(instance).Constructor(delegateType);

        public static Constructor<TSignature> Constructor<TSignature>(this object instance) where TSignature : Delegate =>
            new InstanceMembers(instance).Constructor<TSignature>();

        public static IMembers Declared(this object instance) =>
            new InstanceMembers(instance).DeclaredBy(instance.GetType());

        public static IMembers DeclaredBy(this object instance, Type declaringType) =>
            new InstanceMembers(instance).DeclaredBy(declaringType);

        public static IMembers DeclaredBy<T>(this object instance) =>
            new InstanceMembers(instance).DeclaredBy<T>();

        public static Event Event(this object instance) =>
            new InstanceMembers(instance).Event();

        public static Event Event(this object instance, string eventName) =>
            new InstanceMembers(instance).Event(eventName);

        public static Event Event(this object instance, Type handlerType) =>
            new InstanceMembers(instance).Event(handlerType);

        public static Event Event(this object instance, Type handlerType, string eventName) =>
            new InstanceMembers(instance).Event(handlerType, eventName);

        public static Event<T> Event<T>(this object instance) where T : Delegate =>
            new InstanceMembers(instance).Event<T>();

        public static Event<T> Event<T>(this object instance, string eventName) where T : Delegate =>
            new InstanceMembers(instance).Event<T>(eventName);

        public static Field Field(this object instance) =>
            new InstanceMembers(instance).Field();

        public static Field Field(this object instance, string fieldName) =>
            new InstanceMembers(instance).Field(fieldName);

        public static Field Field(this object instance, Type fieldType) =>
            new InstanceMembers(instance).Field(fieldType);

        public static Field Field(this object instance, Type fieldType, string fieldName) =>
            new InstanceMembers(instance).Field(fieldType, fieldName);

        public static Field<T> Field<T>(this object instance) =>
            new InstanceMembers(instance).Field<T>();

        public static Field<T> Field<T>(this object instance, string fieldName) =>
            new InstanceMembers(instance).Field<T>(fieldName);

        public static IMembers Inherited(this object instance) =>
            new InstanceMembers(instance).InheritedFrom(instance.GetType().BaseType);

        public static IMembers InheritedFrom(this object instance, Type ancestorType) =>
            new InstanceMembers(instance).InheritedFrom(ancestorType);

        public static IMembers InheritedFrom<T>(this object instance) =>
            new InstanceMembers(instance).InheritedFrom<T>();

        public static IMembers Internal(this object instance) =>
            new InstanceMembers(instance).Internal();

        public static Method Method(this object instance) =>
            instance.Declared().Method(); // Declared only because multiple methods are always inherited from Object

        public static Method Method(this object instance, string methodName) =>
            new InstanceMembers(instance).Method(methodName);

        public static Method Method(this object instance, Type methodType) =>
            new InstanceMembers(instance).Method(methodType);

        public static Method Method(this object instance, Type methodType, string methodName) =>
            new InstanceMembers(instance).Method(methodType, methodName);

        public static Method<T> Method<T>(this object instance) where T : Delegate =>
            new InstanceMembers(instance).Method<T>();

        public static Method<T> Method<T>(this object instance, string methodName) where T : Delegate =>
            new InstanceMembers(instance).Method<T>(methodName);

        public static IMembers Private(this object instance) =>
            new InstanceMembers(instance).Private();

        public static Property Property(this object instance) =>
            new InstanceMembers(instance).Property();

        public static Property Property(this object instance, string propertyName) =>
            new InstanceMembers(instance).Property(propertyName);

        public static Property Property(this object instance, Type propertyType) =>
            new InstanceMembers(instance).Property(propertyType);

        public static Property Property(this object instance, Type propertyType, string propertyName) =>
            new InstanceMembers(instance).Property(propertyType, propertyName);

        public static Property<T> Property<T>(this object instance) =>
            new InstanceMembers(instance).Property<T>();

        public static Property<T> Property<T>(this object instance, string propertyName) =>
            new InstanceMembers(instance).Property<T>(propertyName);

        public static IMembers Protected(this object instance) =>
            new InstanceMembers(instance).Protected();

        public static IMembers Public(this object instance) =>
            new InstanceMembers(instance).Public();
    }
}
