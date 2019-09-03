using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class ParameterNameFilterTest
    {
        readonly IFilter<ParameterInfo> parameters = Substitute.For<IFilter<ParameterInfo>>();
        readonly string parameterName = Guid.NewGuid().ToString();

        public class WithName: ParameterNameFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenParametersIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => default(IFilter<ParameterInfo>).WithName(parameterName));
                Assert.Equal("parameters", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenParameterNameIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => parameters.WithName(null));
                Assert.Equal("parameterName", thrown.ParamName);
            }
        }

        public class Implementation: ParameterNameFilterTest
        {
            readonly IFilter<ParameterInfo> sut;

            public Implementation() =>
                sut = parameters.WithName(parameterName);

            [Fact]
            public void ParameterNameReturnsValueGivenToWithName() {
                var implementation = Assert.IsType<ParameterNameFilter.Implementation>(sut);
                Assert.Same(parameterName, implementation.ParameterName);
            }

            [Fact]
            public void PreviousImplementsDecoratorAndReturnsValueGivenToWithName() {
                var decorator = Assert.IsAssignableFrom<IDecorator<IFilter<ParameterInfo>>>(sut);
                Assert.Same(parameters, decorator.Previous);
            }

            [Fact]
            public void GetReturnsParametersWithGivenParameterType() {
                var expected = new ParameterInfo[] { ParameterInfo(parameterName), ParameterInfo(parameterName) };
                var mixed = new ParameterInfo[] { ParameterInfo(), expected[0], ParameterInfo(), expected[1], ParameterInfo() };
                ConfiguredCall arrange = parameters.Get().Returns(mixed);

                IEnumerable<ParameterInfo> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
