using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests of extension methods that return <see cref="Event"/>.
    /// </summary>
    [Collection(nameof(EventExtensionsTest))]
    public class EventExtensionsTest: SelectorFixture<Event>
    {
        // Method parameters
        protected readonly Type handlerType = typeof(TestHandler);
        protected readonly string eventName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Event selected;
        protected IEnumerable<Event>? selection;

        public EventExtensionsTest() {
            selected = new Event(typeof(TestType).GetEvent(nameof(TestType.Event))!, instance);
            object arrange = select.Invoke(Arg.Do<IEnumerable<Event>>(e => selection = e)).Returns(selected);
        }

        internal static MemberNameFilter<Event, EventInfo> VerifyFilter(IEnumerable<Event> selection, string eventName) {
            var filter = (MemberNameFilter<Event, EventInfo>)selection;
            Assert.Equal(eventName, filter.MemberName);
            return filter;
        }

        internal static EventTypeFilter VerifyFilter(IEnumerable<Event> selection, Type expectedHandlerType) {
            var filter = (EventTypeFilter)selection;
            Assert.Equal(expectedHandlerType, filter.HandlerType);
            return filter;
        }

        protected static void VerifyGenericEvent<T>(Event selected, Event<T> generic) where T : Delegate {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        protected class TestType
        {
            public event TestHandler? Event;
        }

        protected class TestEventArgs: EventArgs { }

        protected delegate void TestHandler(object sender, TestEventArgs args);
    }
}
