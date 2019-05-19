using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class EventTypeFilterTest
    {
        readonly IFilter<Event> sut;

        // Constructor parameters
        readonly IFilter<Event> previous = Substitute.For<IFilter<Event>>();
        readonly Type handlerType = Type();

        public EventTypeFilterTest() =>
            sut = new EventTypeFilter(previous, handlerType);

        public class Constructor: EventTypeFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new EventTypeFilter(null, handlerType));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenEventHandlerTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new EventTypeFilter(previous, null));
                Assert.Equal("handlerType", thrown.ParamName);
            }
        }

        public class HandlerType: EventTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(handlerType, ((EventTypeFilter)sut).HandlerType);
        }

        public class Previous: EventTypeFilterTest
        {
            [Fact]
            public void ImplementsDecoratorAndReturnsInstanceGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Event>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get: EventTypeFilterTest
        {
            [Fact]
            public void ReturnsEventsWithGivenHandlerType() {
                // Arrange
                EventInfo fieldInfo = EventInfo(MethodAttributes.Static, handlerType);

                var expected = new[] { new Event(fieldInfo), new Event(fieldInfo) };

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
