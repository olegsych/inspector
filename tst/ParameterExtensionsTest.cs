using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    [Collection(nameof(ParameterExtensionsTest))]
    public class ParameterExtensionsTest: SelectorFixture<ParameterInfo>
    {
        class ParameterType { }

        readonly MethodBase method = MethodBase();
        readonly IMember<MethodBase> member = Substitute.For<IMember<MethodBase>>();
        readonly ParameterInfo parameter = ParameterInfo();
        readonly string parameterName = Guid.NewGuid().ToString();
        readonly Type parameterType = typeof(ParameterType);

        IEnumerable<ParameterInfo> selection;

        public ParameterExtensionsTest() {
            ConfiguredCall arrange;
            arrange = select.Invoke(Arg.Do<IEnumerable<ParameterInfo>>(_ => selection = _)).Returns(parameter);
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
                ParameterTypeFilter.Implementation filter = VerifyFilter(selection, parameterType);
                VerifyParameters(filter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterOfGivenGenericType() {
                Assert.Same(parameter, method.Parameter<ParameterType>());
                ParameterTypeFilter.Implementation filter = VerifyFilter(selection, parameterType);
                VerifyParameters(filter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterWithGivenName() {
                Assert.Same(parameter, method.Parameter(parameterName));
                ParameterNameFilter.Implementation filter = VerifyFilter(selection, parameterName);
                VerifyParameters(filter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterWithGivenTypeAndName() {
                Assert.Same(parameter, method.Parameter(parameterType, parameterName));
                ParameterNameFilter.Implementation nameFilter = VerifyFilter(selection, parameterName);
                ParameterTypeFilter.Implementation typeFilter = VerifyFilter(nameFilter.Previous, parameterType);
                VerifyParameters(typeFilter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterWithGivenGenericTypeAndName() {
                Assert.Same(parameter, method.Parameter<ParameterType>(parameterName));
                ParameterNameFilter.Implementation nameFilter = VerifyFilter(selection, parameterName);
                ParameterTypeFilter.Implementation typeFilter = VerifyFilter(nameFilter.Previous, parameterType);
                VerifyParameters(typeFilter.Previous, method);
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
                ParameterTypeFilter.Implementation filter = VerifyFilter(selection, parameterType);
                VerifyParameters(filter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterWithGivenName() {
                Assert.Same(parameter, member.Parameter(parameterName));
                ParameterNameFilter.Implementation filter = VerifyFilter(selection, parameterName);
                VerifyParameters(filter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterOfGivenGenericType() {
                Assert.Same(parameter, member.Parameter<ParameterType>());
                ParameterTypeFilter.Implementation filter = VerifyFilter(selection, parameterType);
                VerifyParameters(filter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterWithGivenTypeAndName() {
                Assert.Same(parameter, member.Parameter(parameterType, parameterName));
                ParameterNameFilter.Implementation nameFilter = VerifyFilter(selection, parameterName);
                ParameterTypeFilter.Implementation typeFilter = VerifyFilter(nameFilter.Previous, parameterType);
                VerifyParameters(typeFilter.Previous, method);
            }

            [Fact]
            public void ReturnsParameterWithGivenGenericTypeAndName() {
                Assert.Same(parameter, member.Parameter<ParameterType>(parameterName));
                ParameterNameFilter.Implementation nameFilter = VerifyFilter(selection, parameterName);
                ParameterTypeFilter.Implementation typeFilter = VerifyFilter(nameFilter.Previous, parameterType);
                VerifyParameters(typeFilter.Previous, method);
            }
        }

        static void VerifyParameters(IEnumerable<ParameterInfo> selection, MethodBase method) {
            var filter = Assert.IsType<Parameters>(selection);
            Assert.Same(method, filter.Method);
        }

        static ParameterNameFilter.Implementation VerifyFilter(IEnumerable<ParameterInfo> selection, string parameterName) {
            var filter = Assert.IsType<ParameterNameFilter.Implementation>(selection);
            Assert.Equal(parameterName, filter.ParameterName);
            return filter;
        }

        static ParameterTypeFilter.Implementation VerifyFilter(IEnumerable<ParameterInfo> selection, Type parameterType) {
            var filter = Assert.IsType<ParameterTypeFilter.Implementation>(selection);
            Assert.Equal(parameterType, filter.ParameterType);
            return filter;
        }
    }
}
