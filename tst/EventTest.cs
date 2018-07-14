using System;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class EventTest
    {
        readonly Event sut;

        // Constructor parameters
        readonly EventInfo info = typeof(TestType).GetEvent(nameof(TestType.Event));
        readonly object instance = new TestType();

        public EventTest() {
            sut = new Event(info, instance);
        }

        public class Constructor : EventTest
        {
            [Fact]
            public void InitializesBaseType() {
                Member<EventInfo> @base = sut;

                Assert.Same(info, @base.Info);
                Assert.Same(instance, @base.Instance);
            }
        }

        class TestArgs : EventArgs { }

        delegate void TestEvent(object sender, TestArgs args);

        class TestType
        {
            public event TestEvent Event;
        }
    }
}
