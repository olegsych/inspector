using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class EventTest
    {
        readonly Event sut;

        // Constructor parameters
        readonly EventInfo instanceInfo = typeof(TestType).GetEvent(nameof(TestType.Event));
        readonly EventInfo staticInfo = typeof(TestType).GetEvent(nameof(TestType.StaticEvent));
        readonly object instance = new TestType();

        public EventTest() {
            sut = new Event(instanceInfo, instance);
        }

        public class Constructor : EventTest
        {
            [Fact]
            public void InitializesBaseType() {
                Member<EventInfo> @base = sut;

                Assert.Same(instanceInfo, @base.Info);
                Assert.Same(instance, @base.Instance);
            }
        }

        public class Add : EventTest
        {
            [Fact]
            public void AddsEventHandler() {
                var handler = Substitute.For<TestEvent>();

                sut.Add(handler);

                var args = new TestArgs();
                ((TestType)instance).RaiseEvent(args);
                handler.Received().Invoke(instance, args);
            }
        }

        public class Remove : EventTest
        {
            [Fact]
            public void RemovesEventHandler() {
                var handler = Substitute.For<TestEvent>();
                sut.Add(handler);

                sut.Remove(handler);

                ((TestType)instance).RaiseEvent(new TestArgs());
                handler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<TestArgs>());
            }
        }

        public class IsStatic : EventTest
        {
            [Fact]
            public void ReturnsFalseForInstanceEventInfo() =>
                Assert.False(new Event(instanceInfo, instance).IsStatic);

            [Fact]
            public void ReturnsTrueForStaticEventInfo() =>
                Assert.True(new Event(staticInfo, null).IsStatic);
        }

        class TestArgs : EventArgs { }

        delegate void TestEvent(object sender, TestArgs args);

        class TestType
        {
            public event TestEvent Event = (s, a) => { };
            public static event TestEvent StaticEvent;

            public void RaiseEvent(TestArgs args) =>
                Event(this, args);
        }
    }
}
