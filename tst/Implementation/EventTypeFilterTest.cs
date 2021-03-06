using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class EventTypeFilterTest
    {
        readonly Filter<Event> sut;

        // Constructor parameters
        readonly IEnumerable<Event> source = Substitute.For<IEnumerable<Event>>();
        readonly Type handlerType = Type();

        public EventTypeFilterTest() =>
            sut = new EventTypeFilter(source, handlerType);

        public class Constructor: EventTypeFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenSourceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new EventTypeFilter(null!, handlerType));
                Assert.Equal("source", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenEventHandlerTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new EventTypeFilter(source, null!));
                Assert.Equal("handlerType", thrown.ParamName);
            }

            [Fact]
            public void PassesSourceToBaseConstructor() =>
                Assert.Same(source, sut.Source);
        }

        public class HandlerType: EventTypeFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(handlerType, ((EventTypeFilter)sut).HandlerType);
        }

        public class GetEnumerator: EventTypeFilterTest
        {
            [Fact]
            public void ReturnsEventsWithGivenHandlerType() {
                // Arrange
                EventInfo fieldInfo = EventInfo(MethodAttributes.Static, handlerType);

                var expected = new[] { new Event(fieldInfo), new Event(fieldInfo) };

                IEnumerable<Event> mixed = new[] {
                    new Event(EventInfo(MethodAttributes.Static)),
                    expected[0],
                    new Event(EventInfo(MethodAttributes.Static)),
                    expected[1],
                    new Event(EventInfo(MethodAttributes.Static))
                };

                ConfiguredCall arrange = source.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }
        }
    }
}
