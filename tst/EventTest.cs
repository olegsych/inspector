using System;
using System.Reflection;
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
            public event TestEvent Event;
            public static event TestEvent StaticEvent;
        }
    }
}
