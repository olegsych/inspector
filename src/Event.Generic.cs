using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to events of type <typeparamref name="TEventHandler"/>.
    /// </summary>
    /// <typeparam name="TEventHandler">Type of event handler</typeparam>
    public class Event<TEventHandler> : Event where TEventHandler : Delegate
    {
        protected Event(EventInfo info, object instance) : base(null, null) =>
            throw new NotImplementedException();

        public void Add(TEventHandler handler) =>
            throw new NotImplementedException();

        public new TEventHandler Raise =>
            throw new NotImplementedException();

        public void Remove(TEventHandler handler) =>
            throw new NotImplementedException();
    }
}
