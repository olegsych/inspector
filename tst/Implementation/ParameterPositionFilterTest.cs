using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class ParameterPositionFilterTest
    {
        readonly IEnumerable<ParameterInfo> parameters = Substitute.For<IEnumerable<ParameterInfo>>();
        readonly int position = 1;

        public class WithPosition: ParameterPositionFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenSourceIsNull() {
                IEnumerable<ParameterInfo> @null = null!;
                var thrown = Assert.Throws<System.ArgumentNullException>(() => @null.WithPosition(position));
                Assert.Equal("source", thrown.ParamName);
            }
        }

        public class Implementation: ParameterPositionFilterTest
        {
            readonly IEnumerable<ParameterInfo> sut;

            public Implementation() =>
                sut = parameters.WithPosition(position);

            [Fact]
            public void PositionReturnsValueGivenToWithPosition() {
                var implementation = (ParameterPositionFilter.Implementation)sut;
                Assert.Equal(position, implementation.Position);
            }

            [Fact]
            public void SourceImplementsIDecoratorAndReturnsParametersGivenToWithPosition() {
                var decorator = (IDecorator<IEnumerable<ParameterInfo>>)sut;
                Assert.Same(parameters, decorator.Source);
            }

            [Fact]
            public void GetEnumeratorReturnsParametersWithGivenPosition() {
                var expected = new ParameterInfo[] { ParameterInfo(position), ParameterInfo(position) };
                IEnumerable<ParameterInfo> mixed = new ParameterInfo[] { ParameterInfo(0), expected[0], ParameterInfo(0), expected[1], ParameterInfo(0) };
                ConfiguredCall arrange = parameters.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }
        }
    }
}
