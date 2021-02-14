using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class ParametersTest
    {
        readonly IEnumerable<ParameterInfo> sut;

        readonly ParameterInfo[] parameters = new[] { ParameterInfo(), ParameterInfo(), ParameterInfo() };
        readonly MethodBase method;

        public ParametersTest() {
            method = MethodBase(parameters);
            sut = new Parameters(method);
        }

        public class Constructor: ParametersTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Parameters(null!));
                Assert.Equal("method", thrown.ParamName);
            }
        }

        public class Enumerator: ParametersTest
        {
            [Fact]
            public void ReturnsParametersOfMethodGivenToConstructor() =>
                Assert.Equal(parameters, sut.ToArray()); // Force IEnumerable<ParameterInfo>.GetEnumerator call
        }

        public class UntypedEnumerator: ParametersTest
        {
            [Fact]
            public void ReturnsParametersOfMethodGivenToConstructor() =>
                Assert.Equal(parameters, (IEnumerable)sut); // Force IEnumerable.GetEnumerator call
        }

        public class Method: ParametersTest
        {
            [Fact]
            public void ReturnsMethodGivenToConstructor() =>
                Assert.Same(method, ((Parameters)sut).Method);
        }
    }
}
