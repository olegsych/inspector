using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class GenericEventTest
    {
        readonly Event<TestHandler> sut;

        // Constructor parameters
        readonly Event @event;

        // Test Fixture
        readonly TestType instance = new TestType();

        public GenericEventTest() {
            EventInfo info = typeof(TestType).GetEvent(nameof(TestType.Event));
            @event = new Event(info, instance);
            sut = new Event<TestHandler>(@event);
        }

        public class Constructor: GenericEventTest
        {
            [Fact]
            public void InitializesBaseWithEventInfoAndInstance() {
                Assert.Same(@event.Info, sut.Info);
                Assert.Same(@event.Instance, sut.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenGivenEventIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Event<TestHandler>(null));
                Assert.Equal("event", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenGivenEventDoesNotMatchExpectedHandlerType() {
                var thrown = Assert.Throws<ArgumentException>(() => new Event<EventHandler<EventArgs>>(@event));
                Assert.Equal("event", thrown.ParamName);
                Assert.StartsWith($"Event handler type {@event.Info.EventHandlerType} doesn't match expected {typeof(EventHandler<EventArgs>)}.", thrown.Message);
            }
        }

        public class Add: GenericEventTest
        {
            [Fact]
            public void AddsEventHandler() {
                var handler = Substitute.For<TestHandler>();

                sut.Add(handler);

                var args = new TestArgs();
                instance.RaiseEvent(args);
                handler.Received().Invoke(instance, args);
            }
        }

        public class Remove: GenericEventTest
        {
            [Fact]
            public void RemovesEventHandler() {
                var handler = Substitute.For<TestHandler>();
                sut.Add(handler);

                sut.Remove(handler);

                instance.RaiseEvent(new TestArgs());
                handler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<TestArgs>());
            }
        }

        class TestArgs: EventArgs { }

        delegate void TestHandler(object sender, TestArgs args);

        class TestType
        {
            public event TestHandler Event = (s, a) => { };

            public void RaiseEvent(TestArgs a) =>
                Event(this, a);
        }
    }
}
