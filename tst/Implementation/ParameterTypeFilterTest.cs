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
        readonly IFilter<ParameterInfo> parameters = Substitute.For<IFilter<ParameterInfo>>();
        readonly Type parameterType = Type();

        public class WithType: ParameterTypeFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenParametersIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => default(IFilter<ParameterInfo>).WithType(parameterType));
                Assert.Equal("parameters", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenParameterTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => parameters.WithType(null));
                Assert.Equal("parameterType", thrown.ParamName);
            }
        }

        public class Implementation: ParameterTypeFilterTest
        {
            readonly IFilter<ParameterInfo> sut;

            public Implementation() =>
                sut = parameters.WithType(parameterType);

            [Fact]
            public void ParameterTypeReturnsValueGivenToWithType() =>
                Assert.Same(parameterType, ((ParameterTypeFilter.Implementation)sut).ParameterType);

            [Fact]
            public void PreviousImplementsIDecoratorAndRetursParametersGivenToWithType() {
                var decorator = (IDecorator<IFilter<ParameterInfo>>)sut;
                Assert.Same(parameters, decorator.Previous);
            }

            [Fact]
            public void GetReturnsParametersWithGivenParameterType() {
                var expected = new ParameterInfo[] { ParameterInfo(parameterType), ParameterInfo(parameterType) };
                var mixed = new ParameterInfo[] { ParameterInfo(), expected[0], ParameterInfo(), expected[1], ParameterInfo() };
                ConfiguredCall arrange = parameters.Get().Returns(mixed);

                IEnumerable<ParameterInfo> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
