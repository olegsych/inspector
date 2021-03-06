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
        readonly IEnumerable<ParameterInfo> parameters = Substitute.For<IEnumerable<ParameterInfo>>();
        readonly Type parameterType = Type();

        public class WithType: ParameterTypeFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenSourceIsNull() {
                IEnumerable<ParameterInfo> @null = null!;
                var thrown = Assert.Throws<ArgumentNullException>(() => @null.WithType(parameterType));
                Assert.Equal("source", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenParameterTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => parameters.WithType(null!));
                Assert.Equal("parameterType", thrown.ParamName);
            }
        }

        public class Implementation: ParameterTypeFilterTest
        {
            readonly IEnumerable<ParameterInfo> sut;

            public Implementation() =>
                sut = parameters.WithType(parameterType);

            [Fact]
            public void ParameterTypeReturnsValueGivenToWithType() =>
                Assert.Same(parameterType, ((ParameterTypeFilter.Implementation)sut).ParameterType);

            [Fact]
            public void SourceImplementsIDecoratorAndRetursParametersGivenToWithType() {
                var decorator = (IDecorator<IEnumerable<ParameterInfo>>)sut;
                Assert.Same(parameters, decorator.Source);
            }

            [Fact]
            public void GetReturnsParametersWithGivenParameterType() {
                var expected = new ParameterInfo[] { ParameterInfo(parameterType), ParameterInfo(parameterType) };
                IEnumerable<ParameterInfo> mixed = new ParameterInfo[] { ParameterInfo(), expected[0], ParameterInfo(), expected[1], ParameterInfo() };
                ConfiguredCall arrange = parameters.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }
        }
    }
}
