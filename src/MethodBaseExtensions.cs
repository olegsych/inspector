using System;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    public static class MethodBaseExtensions
    {
        public static ParameterInfo Parameter(this MethodBase method, Type parameterType)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            if (parameterType == null)
                throw new ArgumentNullException(nameof(parameterType));

            ParameterInfo parameter = method.GetParameters().SingleOrDefault(_ => _.ParameterType == parameterType);
            if (parameter == null)
            {
                string message = $"{method.DeclaringType.FullName}.{method.Name} doesn't have parameter of type {parameterType.FullName}";
                throw new ArgumentException(message, nameof(parameterType));
            }

            return parameter;
        }
    }
}
