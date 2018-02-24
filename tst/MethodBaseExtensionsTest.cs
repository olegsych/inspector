using System;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class MethodBaseExtensionsTest
    {
        readonly MethodBase method = MethodBase();

        public class Parameter : MethodBaseExtensionsTest
        {
            readonly Type parameterType = Type();

            [Fact]
            public void ReturnsParameterOfGivenType()
            {
                ParameterInfo expected = ParameterInfo(parameterType);
                method.GetParameters().Returns(new[] { expected });

                ParameterInfo actual = method.Parameter(parameterType);

                Assert.Same(expected, actual);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodDoesNotHaveParameterOfGivenType()
            {
                ParameterInfo[] unexpected = { ParameterInfo(), ParameterInfo() };
                method.GetParameters().Returns(unexpected);

                var thrown = Assert.Throws<ArgumentException>(() => method.Parameter(parameterType));

                Assert.Equal("parameterType", thrown.ParamName);
                Assert.Contains(method.DeclaringType.FullName, thrown.Message);
                Assert.Contains(method.Name, thrown.Message);
                Assert.Contains(parameterType.FullName, thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodIsNull()
            {
                var thrown = Assert.Throws<ArgumentNullException>(() => default(MethodBase).Parameter(parameterType));
                Assert.Equal("method", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenParameterTypeIsNull()
            {
                var thrown = Assert.Throws<ArgumentNullException>(() => method.Parameter(null));
                Assert.Equal("parameterType", thrown.ParamName);
            }
        }

        public class ParameterOfT : MethodBaseExtensionsTest
        {
            class P { };

            [Fact]
            public void ReturnsParameterOfGivenType()
            {
                ParameterInfo expected = ParameterInfo(typeof(P));
                method.GetParameters().Returns(new[] { expected });

                ParameterInfo actual = method.Parameter<P>();

                Assert.Same(expected, actual);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodDoesNotHaveParameterOfGivenType()
            {
                ParameterInfo[] unexpected = { ParameterInfo(), ParameterInfo() };
                method.GetParameters().Returns(unexpected);

                var thrown = Assert.Throws<ArgumentException>(() => method.Parameter<P>());

                Assert.Equal("parameterType", thrown.ParamName);
                Assert.Contains(method.DeclaringType.FullName, thrown.Message);
                Assert.Contains(method.Name, thrown.Message);
                Assert.Contains(typeof(P).FullName, thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodIsNull()
            {
                var thrown = Assert.Throws<ArgumentNullException>(() => default(MethodBase).Parameter<P>());
                Assert.Equal("method", thrown.ParamName);
            }
        }
    }
}
