using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class EventTypeFilter : IFilter<Event>, IDecorator<IFilter<Event>>
    {
        public EventTypeFilter(IFilter<Event> previous, Type handlerType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
        }

        public Type HandlerType { get; }

        public IFilter<Event> Previous { get; }

        IEnumerable<Event> IFilter<Event>.Get() =>
            Previous.Get().Where(@event => @event.Info.EventHandlerType == HandlerType);
    }
}
