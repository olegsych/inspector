using System;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting events from a given scope.
    /// </summary>
    public static class EventExtensions
    {
        #region IScope

        public static Event Event(this IScope scope) =>
            Selector<Event>.Select(scope);

        public static Event Event(this IScope scope, string eventName) =>
            Selector<Event>.Select(new EventNameFilter(scope, eventName));

        public static Event Event(this IScope scope, Type handlerType) =>
            Selector<Event>.Select(new EventTypeFilter(scope, handlerType));

        public static Event Event(this IScope scope, Type handlerType, string eventName) {
            var typed = new EventTypeFilter(scope, handlerType);
            var named = new EventNameFilter(typed, eventName);
            return Selector<Event>.Select(named);
        }

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

        #endregion
    }
}
