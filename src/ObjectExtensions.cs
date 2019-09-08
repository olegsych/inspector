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
