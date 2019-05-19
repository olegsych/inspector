using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to events of type not accessible at compile time.
    /// </summary>
    public class Event: Member<EventInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event(EventInfo info, object instance = null) :
            base(info, instance) { }

        internal static Event Create(EventInfo info, object instance) =>
            new Event(info, instance);

        /// <summary>
        /// Returns <c>true</c> when the <see cref="Event"/> is static.
        /// </summary>
        public override bool IsStatic =>
            Info.AddMethod.IsStatic;

        /// <summary>
        /// Adds a handler to the event.
        /// </summary>
        public void Add(Delegate handler) =>
            Info.AddEventHandler(Instance, handler);

        public void Raise(params object[] args) =>
            throw new NotImplementedException();

        /// <summary>
        /// Removes a handler from the event.
        /// </summary>
        public void Remove(Delegate handler) =>
            Info.RemoveEventHandler(Instance, handler);
    }
}
