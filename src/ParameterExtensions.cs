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

        public static ParameterInfo Parameter<TParameterType>(this MethodBase method) =>
            method.Parameter(typeof(TParameterType));

        public static ParameterInfo Parameter<TParameterType>(this MethodBase method, string parameterName) =>
            method.Parameter(typeof(TParameterType), parameterName);

        public static ParameterInfo Parameter(this MethodBase method, Type parameterType) =>
            Selector<ParameterInfo>.Select(new Parameters(method).WithType(parameterType));

        public static ParameterInfo Parameter(this MethodBase method, string parameterName) =>
            Selector<ParameterInfo>.Select(new Parameters(method).WithName(parameterName));

        public static ParameterInfo Parameter(this MethodBase method, Type parameterType, string parameterName) =>
            Selector<ParameterInfo>.Select(new Parameters(method).WithType(parameterType).WithName(parameterName));

        #endregion

        #region IMember<MethodBase>

        public static ParameterInfo Parameter(this IMember<MethodBase> method) =>
            method.Info.Parameter();

        public static ParameterInfo Parameter<TParameterType>(this IMember<MethodBase> method) =>
            method.Info.Parameter<TParameterType>();

        public static ParameterInfo Parameter<TParameterType>(this IMember<MethodBase> method, string parameterName) =>
            method.Info.Parameter<TParameterType>(parameterName);

        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType) =>
            method.Info.Parameter(parameterType);

        public static ParameterInfo Parameter(this IMember<MethodBase> method, string parameterName) =>
            method.Info.Parameter(parameterName);

        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType, string parameterName) =>
            method.Info.Parameter(parameterType, parameterName);

        #endregion
    }
}
