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

        public class IMembersExtension: EventExtensionsTest
        {
            // Method parameters
            readonly IMembers members = Substitute.For<IMembers>();

            // Test fixture
            readonly IEnumerable<Event> events = Substitute.For<IEnumerable<Event>>();

            public IMembersExtension() =>
                members.Events().Returns(events);

            [Fact]
            public void ReturnsSingleEvent() {
                Assert.Same(selected, members.Event());
                Assert.Same(events, selection);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, members.Event(eventName));
                MemberNameFilter<Event, EventInfo> filter = VerifyFilter(selection, eventName);
                Assert.Same(events, filter.Source);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, members.Event(handlerType));
                EventTypeFilter filter = VerifyFilter(selection, handlerType);
                Assert.Same(events, filter.Source);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, members.Event(handlerType, eventName));
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                Assert.Same(events, typed.Source);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = members.Event<TestHandler>();
                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                Assert.Same(events, typed.Source);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = members.Event<TestHandler>(eventName);
                VerifyGenericEvent(selected, generic);
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                Assert.Same(events, typed.Source);
            }
        }

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

        public class TypeExtension: EventExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSingleEventInGivenType() {
                Assert.Same(selected, testType.Event());

                VerifyStaticMembers(selection, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, testType.Event(eventName));

                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                VerifyStaticMembers(named.Source, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, testType.Event(handlerType));

                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyStaticMembers(typed.Source, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, testType.Event(handlerType, eventName));

                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                VerifyStaticMembers(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = testType.Event<TestHandler>();

                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyStaticMembers(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = testType.Event<TestHandler>(eventName);

                VerifyGenericEvent(selected, generic);
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                VerifyStaticMembers(typed.Source, testType);
            }

            static void VerifyStaticMembers(IEnumerable<Event> selection, Type type) {
                var events = Assert.IsType<Members<EventInfo, Event>>(selection);
                Assert.Same(type, events.Type);
            }
        }
    }
}
