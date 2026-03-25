using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting parameters from methods and constructors.
    /// </summary>
    public static class ParameterExtensions
    {
        #region MethodBase

        /// <summary>
        /// Returns the only parameter of the method.
        /// </summary>
        public static ParameterInfo Parameter(this MethodBase method) =>
            new Parameters(method).Single();

        /// <summary>
        /// Returns the only parameter of the specified type.
        /// </summary>
        public static ParameterInfo Parameter<TParameterType>(this MethodBase method) =>
            method.Parameter(typeof(TParameterType));

        /// <summary>
        /// Returns the parameter of the specified type and name.
        /// </summary>
        public static ParameterInfo Parameter<TParameterType>(this MethodBase method, string parameterName) =>
            method.Parameter(typeof(TParameterType), parameterName);

        /// <summary>
        /// Returns the only parameter of the specified type.
        /// </summary>
        public static ParameterInfo Parameter(this MethodBase method, Type parameterType) =>
            new Parameters(method).WithType(parameterType).Single();

        /// <summary>
        /// Returns the parameter with the specified name.
        /// </summary>
        public static ParameterInfo Parameter(this MethodBase method, string parameterName) =>
            new Parameters(method).WithName(parameterName).Single();

        /// <summary>
        /// Returns the parameter of the specified type and name.
        /// </summary>
        public static ParameterInfo Parameter(this MethodBase method, Type parameterType, string parameterName) =>
            new Parameters(method).WithType(parameterType).WithName(parameterName).Single();

        #endregion

        #region IMember<MethodBase>

        /// <summary>
        /// Returns the only parameter of the method.
        /// </summary>
        public static ParameterInfo Parameter(this IMember<MethodBase> method) =>
            method.Info.Parameter();

        /// <summary>
        /// Returns the only parameter of the specified type.
        /// </summary>
        public static ParameterInfo Parameter<TParameterType>(this IMember<MethodBase> method) =>
            method.Info.Parameter<TParameterType>();

        /// <summary>
        /// Returns the parameter of the specified type and name.
        /// </summary>
        public static ParameterInfo Parameter<TParameterType>(this IMember<MethodBase> method, string parameterName) =>
            method.Info.Parameter<TParameterType>(parameterName);

        /// <summary>
        /// Returns the only parameter of the specified type.
        /// </summary>
        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType) =>
            method.Info.Parameter(parameterType);

        /// <summary>
        /// Returns the parameter with the specified name.
        /// </summary>
        public static ParameterInfo Parameter(this IMember<MethodBase> method, string parameterName) =>
            method.Info.Parameter(parameterName);

        /// <summary>
        /// Returns the parameter of the specified type and name.
        /// </summary>
        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType, string parameterName) =>
            method.Info.Parameter(parameterType, parameterName);

        #endregion
    }
}
