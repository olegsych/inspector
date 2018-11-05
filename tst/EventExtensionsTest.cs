using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests of extension methods that return <see cref="Event"/>.
    /// </summary>
    [Collection(nameof(EventExtensionsTest))]
    public class EventExtensionsTest : SelectorFixture<Event>
    {
        // Method parameters
        protected readonly Type handlerType = typeof(TestHandler);
        protected readonly string eventName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Event selected;
        protected IFilter<Event> selection;

        public EventExtensionsTest() {
            selected = new Event(typeof(TestType).GetEvent(nameof(TestType.Event)), instance);
            select.Invoke(Arg.Do<IFilter<Event>>(e => selection = e)).Returns(selected);
        }

        internal static EventNameFilter VerifyFilter(IFilter<Event> selection, string eventName) {
            var filter = Assert.IsType<EventNameFilter>(selection);
            Assert.Equal(eventName, filter.EventName);
            return filter;
        }

        internal static EventTypeFilter VerifyFilter(IFilter<Event> selection, Type expectedHandlerType) {
            var filter = Assert.IsType<EventTypeFilter>(selection);
            Assert.Equal(expectedHandlerType, filter.HandlerType);
            return filter;
        }

        protected static void VerifyGenericEvent<T>(Event selected, Event<T> generic) where T: Delegate {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        protected class TestType
        {
            public event TestHandler Event;
        }

        protected class TestEventArgs : EventArgs { }

        protected delegate void TestHandler(object sender, TestEventArgs args);

        public class IScopeExtension : EventExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void ReturnsSingleEventInGivenScope() {
                Assert.Same(selected, scope.Event());

                Assert.Same(scope, selection);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, scope.Event(eventName));

                EventNameFilter filter = VerifyFilter(selection, eventName);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, scope.Event(handlerType));

                EventTypeFilter filter = VerifyFilter(selection, handlerType);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, scope.Event(handlerType, eventName));

                EventNameFilter named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Previous, handlerType);
                Assert.Same(scope, typed.Previous);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = scope.Event<TestHandler>();

                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                Assert.Same(scope, typed.Previous);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = scope.Event<TestHandler>(eventName);

                VerifyGenericEvent(selected, generic);
                EventNameFilter named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Previous, handlerType);
                Assert.Same(scope, typed.Previous);
            }
        }

        public class ObjectExtension : EventExtensionsTest
        {
            [Fact]
            public void ReturnsSingleEventOfGivenInstance() {
                Assert.Same(selected, instance.Event());

                VerifyScope(selection, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, instance.Event(eventName));

                EventNameFilter named = VerifyFilter(selection, eventName);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, instance.Event(handlerType));

                EventTypeFilter named = VerifyFilter(selection, handlerType);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, instance.Event(handlerType, eventName));

                EventNameFilter named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Previous, handlerType);
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = instance.Event<TestHandler>();

                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = instance.Event<TestHandler>(eventName);

                VerifyGenericEvent(selected, generic);
                EventNameFilter named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Previous, handlerType);
                VerifyScope(typed.Previous, instance);
            }

            static void VerifyScope(IFilter<Event> filter, object instance) {
                var scope = Assert.IsType<InstanceScope>(filter);
                Assert.Same(instance, scope.Instance);
            }
        }

        public class TypeExtension : EventExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSingleEventInGivenType() {
                Assert.Same(selected, testType.Event());

                VerifyScope(selection, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, testType.Event(eventName));

                EventNameFilter named = VerifyFilter(selection, eventName);
                VerifyScope(named.Previous, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, testType.Event(handlerType));

                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, testType.Event(handlerType, eventName));

                EventNameFilter named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Previous, handlerType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = testType.Event<TestHandler>();

                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = testType.Event<TestHandler>(eventName);

                VerifyGenericEvent(selected, generic);
                EventNameFilter named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Previous, handlerType);
                VerifyScope(typed.Previous, testType);
            }

            static void VerifyScope(IFilter<Event> selection, Type type) {
                var scope = Assert.IsType<StaticScope>(selection);
                Assert.Same(type, scope.Type);
            }
        }
    }
}
