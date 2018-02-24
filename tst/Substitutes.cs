using System;
using System.Reflection;
using NSubstitute;

namespace Inspector
{
    static class Substitutes
    {
        static uint seed = 1;

        static uint Next => seed++;

        public static MethodBase MethodBase()
        {
            var method = Substitute.For<MethodBase>();
            Type declaringType = Type();
            method.DeclaringType.Returns(declaringType);
            method.Name.Returns($"Method{Next}");
            method.GetParameters().Returns(new ParameterInfo[0]);
            return method;
        }

        public static ParameterInfo ParameterInfo(Type parameterType = default)
        {
            parameterType = parameterType ?? Type();
            var parameter = Substitute.For<ParameterInfo>();
            parameter.ParameterType.Returns(parameterType);
            return parameter;
        }

        public static Type Type()
        {
            var type = Substitute.For<Type>();
            type.Name.Returns($"Type{Next}");
            type.Namespace.Returns($"Namespace{Next}");
            string fullName = $"{$"Namespace{Next}"}.{$"Type{Next}"}";
            type.FullName.Returns(fullName);
            return type;
        }
    }
}
