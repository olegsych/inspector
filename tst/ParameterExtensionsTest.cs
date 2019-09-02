using System;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class ParameterExtensionsTest: SelectorFixture<ParameterInfo>
    {
        class ParameterType { }

        readonly MethodBase method = MethodBase();
        readonly IMember<MethodBase> member = Substitute.For<IMember<MethodBase>>();
        readonly ParameterInfo parameter = ParameterInfo();
        readonly Type parameterType = typeof(ParameterType);

        IFilter<ParameterInfo> selection;

        public ParameterExtensionsTest() {
            ConfiguredCall arrange;
            arrange = select.Invoke(Arg.Do<IFilter<ParameterInfo>>(_ => selection = _)).Returns(parameter);
            arrange = member.Info.Returns(method);
        }

        public class MethodBaseParameter: ParameterExtensionsTest
        {
            [Fact]
            public void ReturnsSingleParameter() {
                Assert.Same(parameter, method.Parameter());
                VerifyParameters(selection, method);
            }

            [Fact]
            public void ReturnsParameterOfGivenType() {
                Assert.Same(parameter, method.Parameter(parameterType));
                ParameterTypeFilter filter = VerifyFilter(selection, parameterType);
                VerifyParameters(filter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterOfGivenGenericType() {
                Assert.Same(parameter, method.Parameter<ParameterType>());
                ParameterTypeFilter filter = VerifyFilter(selection, parameterType);
                VerifyParameters(filter.Previous, method);
            }
        }

        public class IMemberOfMethodBaseParameter: ParameterExtensionsTest
        {
            [Fact]
            public void ReturnsSingleParameter() {
                Assert.Same(parameter, member.Parameter());
                VerifyParameters(selection, method);
            }

            [Fact]
            public void ReturnsParameterOfGivenType() {
                Assert.Same(parameter, member.Parameter(parameterType));
                ParameterTypeFilter filter = VerifyFilter(selection, parameterType);
                VerifyParameters(filter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterOfGivenGenericType() {
                Assert.Same(parameter, member.Parameter<ParameterType>());
                ParameterTypeFilter filter = VerifyFilter(selection, parameterType);
                VerifyParameters(filter.Previous, method);
            }
        }

        static void VerifyParameters(IFilter<ParameterInfo> selection, MethodBase method) {
            var filter = Assert.IsType<Parameters>(selection);
            Assert.Same(method, filter.Method);
        }

        static ParameterTypeFilter VerifyFilter(IFilter<ParameterInfo> selection, Type parameterType) {
            var filter = Assert.IsType<ParameterTypeFilter>(selection);
            Assert.Equal(parameterType, filter.ParameterType);
            return filter;
        }
    }
}
