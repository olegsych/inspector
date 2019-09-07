using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    sealed class EventTypeFilter: Filter<Event>
    {
        public EventTypeFilter(IEnumerable<Event> source, Type handlerType): base(source) =>
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));

        public Type HandlerType { get; }

        public override IEnumerator<Event> GetEnumerator() =>
            Source.Where(@event => @event.Info.EventHandlerType == HandlerType).GetEnumerator();
    }
}
