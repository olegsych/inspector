using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting events from a given scope.
    /// </summary>
    public static class EventExtensions
    {
        #region IScope

        public static Event Event(this IScope scope) =>
            scope.Events().Single();

        public static Event Event(this IScope scope, string eventName) =>
            new MemberNameFilter<Event, EventInfo>(scope.Events(), eventName).Single();

        public static Event Event(this IScope scope, Type handlerType) =>
            new EventTypeFilter(scope.Events(), handlerType).Single();

        public static Event Event(this IScope scope, Type handlerType, string eventName) {
            var typed = new EventTypeFilter(scope.Events(), handlerType);
            var named = new MemberNameFilter<Event, EventInfo>(typed, eventName);
            return named.Single();
        }

        public static Event<T> Event<T>(this IScope scope) where T : Delegate =>
            new Event<T>(scope.Event(typeof(T)));

        public static Event<T> Event<T>(this IScope scope, string eventName) where T : Delegate =>
            new Event<T>(scope.Event(typeof(T), eventName));

        #endregion

        #region Object

        public static Event Event(this object instance) =>
            new InstanceScope(instance).Event();

        public static Event Event(this object instance, string eventName) =>
            new InstanceScope(instance).Event(eventName);

        public static Event Event(this object instance, Type handlerType) =>
            new InstanceScope(instance).Event(handlerType);

        public static Event Event(this object instance, Type handlerType, string eventName) =>
            new InstanceScope(instance).Event(handlerType, eventName);

        public static Event<T> Event<T>(this object instance) where T : Delegate =>
            new InstanceScope(instance).Event<T>();

        public static Event<T> Event<T>(this object instance, string eventName) where T : Delegate =>
            new InstanceScope(instance).Event<T>(eventName);

        #endregion

        #region Type

        public static Event Event(this Type type) =>
            new StaticScope(type).Event();

        public static Event Event(this Type type, string eventName) =>
            new StaticScope(type).Event(eventName);

        public static Event Event(this Type type, Type handlerType) =>
            new StaticScope(type).Event(handlerType);

        public static Event Event(this Type type, Type handlerType, string eventName) =>
            new StaticScope(type).Event(handlerType, eventName);

        public static Event<T> Event<T>(this Type type) where T : Delegate =>
            new StaticScope(type).Event<T>();

        public static Event<T> Event<T>(this Type type, string eventName) where T : Delegate =>
            new StaticScope(type).Event<T>(eventName);

        #endregion
    }
}
