using System;

namespace Inspector
{
    /// <summary>
    /// Provides access to events of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of event handler</typeparam>
    public class Event<T>: Event where T : Delegate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event{T}"/> class.
        /// </summary>
        public Event(Event @event) : base(NotNull(@event).Info, @event.Instance) {
            if(@event.Info.EventHandlerType != typeof(T))
                throw new ArgumentException($"Event handler type {@event.Info.EventHandlerType} doesn't match expected {typeof(T)}.", nameof(@event));
        }

        /// <summary>
        /// Adds a handler to the event.
        /// </summary>
        public void Add(T handler) =>
            base.Add(handler);

        public new T Raise =>
            throw new NotImplementedException();

        /// <summary>
        /// Removes a handler from the event.
        /// </summary>
        public void Remove(T handler) =>
            base.Remove(handler);

        static Event NotNull(Event @event) =>
            @event ?? throw new ArgumentNullException(nameof(@event));
    }
}
