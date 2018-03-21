using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to events of type not accessible at compile time.
    /// </summary>
    public class Event : Member<EventInfo>
    {
        protected Event(EventInfo info, object instance) : base(null, null) =>
            throw new NotImplementedException();

        public void Add(Delegate handler) =>
            throw new NotImplementedException();

        public void Raise(params object[] args) =>
            throw new NotImplementedException();

        public void Remove(Delegate handler) =>
            throw new NotImplementedException();
    }
}
