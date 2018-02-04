using System;
using System.Reflection;
using NSubstitute;

namespace Inspector
{
    static class Substitutes
    {
        public static ParameterInfo ParameterInfo(Type parameterType)
        {
            var parameter = Substitute.For<ParameterInfo>();
            parameter.ParameterType.Returns(parameterType);
            return parameter;
        }
    }
}
