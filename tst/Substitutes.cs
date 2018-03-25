using System;
using System.Reflection;
using NSubstitute;

namespace Inspector
{
    static class Substitutes
    {
        static uint seed = 1;

        static uint Next => seed++;

        public static FieldInfo FieldInfo(FieldAttributes attributes, string fieldName) =>
            FieldInfo(attributes, null, fieldName);

        public static FieldInfo FieldInfo(FieldAttributes attributes, Type fieldType = default, string fieldName = default) {
            fieldType = fieldType ?? Type();
            fieldName = fieldName ?? $"Field{Next}";
            var field = Substitute.For<FieldInfo>();
            field.Attributes.Returns(attributes);
            field.FieldType.Returns(fieldType);
            field.Name.Returns(fieldName);
            return field;
        }

        public static MethodBase MethodBase() {
            var method = Substitute.For<MethodBase>();
            Type declaringType = Type();
            method.DeclaringType.Returns(declaringType);
            method.Name.Returns($"Method{Next}");
            method.GetParameters().Returns(new ParameterInfo[0]);
            return method;
        }

        public static ParameterInfo ParameterInfo(Type parameterType = default) {
            parameterType = parameterType ?? Type();
            var parameter = Substitute.For<ParameterInfo>();
            parameter.ParameterType.Returns(parameterType);
            return parameter;
        }

        public static Type Type() {
            var type = Substitute.For<Type>();
            type.Name.Returns($"Type{Next}");
            type.Namespace.Returns($"Namespace{Next}");
            string fullName = $"{$"Namespace{Next}"}.{$"Type{Next}"}";
            type.FullName.Returns(fullName);
            return type;
        }
    }
}
