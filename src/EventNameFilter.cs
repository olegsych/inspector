using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class EventNameFilter : IFilter<Event>, IDecorator<IFilter<Event>>
    {
        public EventNameFilter(IFilter<Event> previous, string eventName) {
            Previous = previous ?? throw new System.ArgumentNullException(nameof(previous));
            EventName = eventName ?? throw new System.ArgumentNullException(nameof(eventName));
        }

        public IFilter<Event> Previous { get; }

        public string EventName { get; }

        IEnumerable<Event> IFilter<Event>.Get() =>
            Previous.Get().Where(@event => @event.Info.Name == EventName);
    }
}
