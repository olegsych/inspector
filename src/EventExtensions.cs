using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting events.
    /// </summary>
    public static class EventExtensions
    {
        #region IMembers

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

        #endregion

        #region Object

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

        #endregion

        #region Type

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

        #endregion
    }
}
