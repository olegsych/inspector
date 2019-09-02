using System;
using System.Reflection;

namespace Inspector
{
    public static class IMemberOfMethodBaseExtensions
    {
        public static ParameterInfo Parameter<TParameterType>(this IMember<MethodBase> method) =>
            throw new NotImplementedException();

        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType) =>
            throw new NotImplementedException();

        public static ParameterInfo Parameter(this IMember<MethodBase> method, string parameterName) =>
            throw new NotImplementedException();
    }
}
