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
        readonly IEnumerable<ParameterInfo> parameters = Substitute.For<IEnumerable<ParameterInfo>>();
        readonly string parameterName = Guid.NewGuid().ToString();

        public class WithName: ParameterNameFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => default(IEnumerable<ParameterInfo>).WithName(parameterName));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenParameterNameIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => parameters.WithName(null));
                Assert.Equal("parameterName", thrown.ParamName);
            }
        }

        public class Implementation: ParameterNameFilterTest
        {
            readonly IEnumerable<ParameterInfo> sut;

            public Implementation() =>
                sut = parameters.WithName(parameterName);

            [Fact]
            public void ParameterNameReturnsValueGivenToWithName() {
                var implementation = Assert.IsType<ParameterNameFilter.Implementation>(sut);
                Assert.Same(parameterName, implementation.ParameterName);
            }

            [Fact]
            public void PreviousImplementsDecoratorAndReturnsValueGivenToWithName() {
                var decorator = Assert.IsAssignableFrom<IDecorator<IEnumerable<ParameterInfo>>>(sut);
                Assert.Same(parameters, decorator.Previous);
            }

            [Fact]
            public void GetEnumeratorReturnsParametersWithGivenParameterType() {
                var expected = new ParameterInfo[] { ParameterInfo(parameterName), ParameterInfo(parameterName) };
                IEnumerable<ParameterInfo> mixed = new ParameterInfo[] { ParameterInfo(), expected[0], ParameterInfo(), expected[1], ParameterInfo() };
                ConfiguredCall arrange = parameters.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }
        }
    }
}
