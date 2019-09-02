using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class ParameterTypeFilterTest
    {
        readonly IFilter<ParameterInfo> sut;

        readonly IFilter<ParameterInfo> previous = Substitute.For<IFilter<ParameterInfo>>();
        readonly Type parameterType = Type();

        public ParameterTypeFilterTest() =>
            sut = new ParameterTypeFilter(previous, parameterType);

        public class Constructor: ParameterTypeFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new ParameterTypeFilter(null, parameterType));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenParameterTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new ParameterTypeFilter(previous, null));
                Assert.Equal("parameterType", thrown.ParamName);
            }
        }

        public class Previous: ParameterTypeFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndRetursPreviousFilterGivenToConstructor() {
                var decorator = (IDecorator<IFilter<ParameterInfo>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class ParameterType: ParameterTypeFilterTest
        {
            [Fact]
            public void ReturnsParameterTypeGivenToConstructor() =>
                Assert.Same(parameterType, ((ParameterTypeFilter)sut).ParameterType);
        }

        public class Get: ParameterTypeFilterTest
        {
            [Fact]
            public void ReturnsParametersWithGivenParameterType() {
                var expected = new ParameterInfo[] { ParameterInfo(parameterType), ParameterInfo(parameterType) };
                var mixed = new ParameterInfo[] { ParameterInfo(), expected[0], ParameterInfo(), expected[1], ParameterInfo() };
                ConfiguredCall arrange = previous.Get().Returns(mixed);

                IEnumerable<ParameterInfo> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
