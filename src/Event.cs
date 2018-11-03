using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to events of type not accessible at compile time.
    /// </summary>
    public class Event : Member<EventInfo>
    {
        public Event(EventInfo info, object instance = null) :
            base(info, instance) { }

        internal static Event Create(EventInfo info, object instance) =>
            new Event(info, instance);

        public override bool IsStatic =>
            Info.AddMethod.IsStatic;

        public void Add(Delegate handler) =>
            Info.AddEventHandler(Instance, handler);

        public void Raise(params object[] args) =>
            throw new NotImplementedException();

        public void Remove(Delegate handler) =>
            Info.RemoveEventHandler(Instance, handler);
    }
}
