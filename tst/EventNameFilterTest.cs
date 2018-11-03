using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class EventNameFilterTest
    {
        readonly IFilter<Event> sut;

        // Constructor parameters
        readonly IFilter<Event> previous = Substitute.For<IFilter<Event>>();
        readonly string eventName = Guid.NewGuid().ToString();

        public EventNameFilterTest() =>
            sut = new EventNameFilter(previous, eventName);

        public class Constructor : EventNameFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new EventNameFilter(null, eventName));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenEventNameIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new EventNameFilter(previous, null));
                Assert.Equal("eventName", thrown.ParamName);
            }
        }

        public class EventName : EventNameFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(eventName, ((EventNameFilter)sut).EventName);
        }

        public class Previous : EventNameFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsFilterGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Event>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get : EventNameFilterTest
        {
            [Fact]
            public void ReturnsEventsWithGivenName() {
                // Arrange
                EventInfo eventInfo = EventInfo(MethodAttributes.Static, eventName);

                var expected = new[] { new Event(eventInfo), new Event(eventInfo) };

                var mixed = new[] {
                    new Event(EventInfo(MethodAttributes.Static)),
                    expected[0],
                    new Event(EventInfo(MethodAttributes.Static)),
                    expected[1],
                    new Event(EventInfo(MethodAttributes.Static))
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Event> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
