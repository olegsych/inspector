using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    sealed class EventTypeFilter: Filter<Event>
    {
        public EventTypeFilter(IEnumerable<Event> previous, Type handlerType): base(previous) =>
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));

        public Type HandlerType { get; }

        public override IEnumerator<Event> GetEnumerator() =>
            Previous.Where(@event => @event.Info.EventHandlerType == HandlerType).GetEnumerator();
    }
}
