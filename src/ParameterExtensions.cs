using System;
using System.Linq;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    public static class ParameterExtensions
    {
        #region MethodBase

        public static ParameterInfo Parameter(this MethodBase method) =>
            Selector<ParameterInfo>.Select(new Parameters(method));

        public static ParameterInfo Parameter<T>(this MethodBase method) =>
            method.Parameter(typeof(T));

        public static ParameterInfo Parameter<T>(this MethodBase method, string parameterName) =>
            throw new NotImplementedException();

        public static ParameterInfo Parameter(this MethodBase method, Type parameterType) =>
            Selector<ParameterInfo>.Select(new ParameterTypeFilter(new Parameters(method), parameterType));

        public static ParameterInfo Parameter(this MethodBase method, string parameterName) =>
            throw new NotImplementedException();

        public static ParameterInfo Parameter(this MethodBase method, Type parameterType, string parameterName) =>
            throw new NotImplementedException();

        #endregion

        #region IMember<MethodBase>

        public static ParameterInfo Parameter(this IMember<MethodBase> method) =>
            method.Info.Parameter();

        public static ParameterInfo Parameter<TParameterType>(this IMember<MethodBase> method) =>
            method.Info.Parameter<TParameterType>();

        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType) =>
            method.Info.Parameter(parameterType);

        public static ParameterInfo Parameter(this IMember<MethodBase> method, string parameterName) =>
            throw new NotImplementedException();

        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType, string parameterName) =>
            throw new NotImplementedException();

        #endregion
    }
}
