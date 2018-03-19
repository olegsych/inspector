using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to events of type not accessible at compile time.
    /// </summary>
    public class Event : Member<EventInfo>
    {
        protected Event(EventInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public void Add(Delegate handler) => throw new NotImplementedException();
        public void Raise(params object[] args) => throw new NotImplementedException();
        public void Remove(Delegate handler) => throw new NotImplementedException();
    }

    /// <summary>
    /// Provides access to events of type <typeparamref name="TEventHandler"/>.
    /// </summary>
    /// <typeparam name="TEventHandler">Type of event handler</typeparam>
    public class Event<TEventHandler> : Event
    {
        protected Event(EventInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public void Add(TEventHandler handler) => throw new NotImplementedException();
        public TEventHandler Raise => throw new NotImplementedException();
        public void Remove(TEventHandler handler) => throw new NotImplementedException();
    }
}
