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
        /// Returns the parameter of the specified type at the specified position.
        /// </summary>
        public static ParameterInfo Parameter<TParameterType>(this MethodBase method, int position) =>
            method.Parameter(typeof(TParameterType), position);

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
        /// Returns the parameter at the specified position.
        /// </summary>
        public static ParameterInfo Parameter(this MethodBase method, int position) =>
            new Parameters(method).WithPosition(position).Single();

        /// <summary>
        /// Returns the parameter of the specified type and name.
        /// </summary>
        public static ParameterInfo Parameter(this MethodBase method, Type parameterType, string parameterName) =>
            new Parameters(method).WithType(parameterType).WithName(parameterName).Single();

        /// <summary>
        /// Returns the parameter of the specified type at the specified position.
        /// </summary>
        public static ParameterInfo Parameter(this MethodBase method, Type parameterType, int position) =>
            new Parameters(method).WithType(parameterType).WithPosition(position).Single();

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
        /// Returns the parameter of the specified type at the specified position.
        /// </summary>
        public static ParameterInfo Parameter<TParameterType>(this IMember<MethodBase> method, int position) =>
            method.Info.Parameter<TParameterType>(position);

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
        /// Returns the parameter at the specified position.
        /// </summary>
        public static ParameterInfo Parameter(this IMember<MethodBase> method, int position) =>
            method.Info.Parameter(position);

        /// <summary>
        /// Returns the parameter of the specified type and name.
        /// </summary>
        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType, string parameterName) =>
            method.Info.Parameter(parameterType, parameterName);

        /// <summary>
        /// Returns the parameter of the specified type at the specified position.
        /// </summary>
        public static ParameterInfo Parameter(this IMember<MethodBase> method, Type parameterType, int position) =>
            method.Info.Parameter(parameterType, position);

        #endregion
    }
}
