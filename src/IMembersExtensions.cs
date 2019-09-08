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
        static readonly IDelegateFactory<ConstructorInfo> delegateFactory = new ConstructorDelegateFactory();

        public static Constructor Constructor(this IMembers members) =>
            members.Constructors().Single();

        public static Constructor Constructor(this IMembers members, Type delegateType) =>
            new ConstructorTypeFilter(members.Constructors(), delegateType, delegateFactory).Single();

        public static Constructor<TSignature> Constructor<TSignature>(this IMembers members) where TSignature : Delegate =>
            new Constructor<TSignature>(members.Constructor(typeof(TSignature)), delegateFactory);

        public static IMembers DeclaredBy(this IMembers members, Type declaringType) =>
            new DeclaredMembers(members, declaringType);

        public static IMembers DeclaredBy<T>(this IMembers members) =>
            new DeclaredMembers(members, typeof(T));

        public static Event Event(this IMembers members) =>
            members.Events().Single();

        public static Event Event(this IMembers members, string eventName) =>
            new MemberNameFilter<Event, EventInfo>(members.Events(), eventName).Single();

        public static Event Event(this IMembers members, Type handlerType) =>
            new EventTypeFilter(members.Events(), handlerType).Single();

        public static Event Event(this IMembers members, Type handlerType, string eventName) {
            var typed = new EventTypeFilter(members.Events(), handlerType);
            var named = new MemberNameFilter<Event, EventInfo>(typed, eventName);
            return named.Single();
        }

        public static Event<T> Event<T>(this IMembers members) where T : Delegate =>
            new Event<T>(members.Event(typeof(T)));

        public static Event<T> Event<T>(this IMembers members, string eventName) where T : Delegate =>
            new Event<T>(members.Event(typeof(T), eventName));

        public static Field Field(this IMembers members) =>
            members.Fields().Single();

        public static Field Field(this IMembers members, string fieldName) =>
            new MemberNameFilter<Field, FieldInfo>(members.Fields(), fieldName).Single();

        public static Field Field(this IMembers members, Type fieldType) =>
            new FieldTypeFilter(members.Fields(), fieldType).Single();

        public static Field Field(this IMembers members, Type fieldType, string fieldName) {
            var typed = new FieldTypeFilter(members.Fields(), fieldType);
            var named = new MemberNameFilter<Field, FieldInfo>(typed, fieldName);
            return named.Single();
        }

        public static Field<T> Field<T>(this IMembers members) =>
            new Field<T>(members.Field(typeof(T)));

        public static Field<T> Field<T>(this IMembers members, string fieldName) =>
            new Field<T>(members.Field(typeof(T), fieldName));

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
