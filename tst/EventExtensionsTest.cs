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
        protected IEnumerable<Event> selection;

        public EventExtensionsTest() {
            selected = new Event(typeof(TestType).GetEvent(nameof(TestType.Event)), instance);
            select.Invoke(Arg.Do<IEnumerable<Event>>(e => selection = e)).Returns(selected);
        }

        internal static MemberNameFilter<Event, EventInfo> VerifyFilter(IEnumerable<Event> selection, string eventName) {
            var filter = Assert.IsType<MemberNameFilter<Event, EventInfo>>(selection);
            Assert.Equal(eventName, filter.MemberName);
            return filter;
        }

        internal static EventTypeFilter VerifyFilter(IEnumerable<Event> selection, Type expectedHandlerType) {
            var filter = Assert.IsType<EventTypeFilter>(selection);
            Assert.Equal(expectedHandlerType, filter.HandlerType);
            return filter;
        }

        protected static void VerifyGenericEvent<T>(Event selected, Event<T> generic) where T : Delegate {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        protected class TestType
        {
            public event TestHandler Event;
        }

        protected class TestEventArgs: EventArgs { }

        protected delegate void TestHandler(object sender, TestEventArgs args);

        public class ObjectExtension: EventExtensionsTest
        {
            [Fact]
            public void ReturnsSingleEventOfGivenInstance() {
                Assert.Same(selected, instance.Event());

                VerifyInstanceMembers(selection, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, instance.Event(eventName));

                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                VerifyInstanceMembers(named.Source, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, instance.Event(handlerType));

                EventTypeFilter named = VerifyFilter(selection, handlerType);
                VerifyInstanceMembers(named.Source, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, instance.Event(handlerType, eventName));

                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                VerifyInstanceMembers(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = instance.Event<TestHandler>();

                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyInstanceMembers(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = instance.Event<TestHandler>(eventName);

                VerifyGenericEvent(selected, generic);
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                VerifyInstanceMembers(typed.Source, instance);
            }

            static void VerifyInstanceMembers(IEnumerable<Event> filter, object instance) {
                var events = Assert.IsType<Members<EventInfo, Event>>(filter);
                Assert.Same(instance, events.Instance);
            }
        }
    }
}
