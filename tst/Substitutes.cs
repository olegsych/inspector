using System;
using System.Reflection;
using NSubstitute;

namespace Inspector
{
    static class Substitutes
    {
        static uint seed = 1;

        static uint Next => seed++;

        static object? arrange;

        public static ConstructorInfo ConstructorInfo(MethodAttributes attributes) {
            var constructor = Substitute.For<ConstructorInfo>();
            arrange = constructor.Attributes.Returns(attributes);
            return constructor;
        }

        public static EventInfo EventInfo(MethodAttributes attributes, Type? handlerType = default, string? eventName = default) {
            handlerType = handlerType ?? Type();
            eventName = eventName ?? $"Event{Next}";
            var @event = Substitute.For<EventInfo>();
            MethodInfo addMethod = MethodInfo(attributes);
            arrange = @event.AddMethod.Returns(addMethod);
            arrange = @event.EventHandlerType.Returns(handlerType);
            arrange = @event.Name.Returns(eventName);
            return @event;
        }

        public static FieldInfo FieldInfo(FieldAttributes attributes, Type? fieldType = default, string? fieldName = default) {
            fieldType = fieldType ?? Type();
            fieldName = fieldName ?? $"Field{Next}";
            var field = Substitute.For<FieldInfo>();
            arrange = field.Attributes.Returns(attributes);
            arrange = field.FieldType.Returns(fieldType);
            arrange = field.Name.Returns(fieldName);
            return field;
        }

        public static MethodBase MethodBase(params ParameterInfo[] parameters) {
            parameters = parameters ?? new ParameterInfo[0];
            var method = Substitute.For<MethodBase>();
            Type declaringType = Type();
            arrange = method.DeclaringType.Returns(declaringType);
            arrange = method.Name.Returns($"Method{Next}");
            arrange = method.GetParameters().Returns(parameters);
            return method;
        }

        public static MethodInfo MethodInfo(MethodAttributes attributes, string? name = default) {
            name = name ?? $"Method{Next}";
            var method = Substitute.For<MethodInfo>();
            arrange = method.Attributes.Returns(attributes);
            arrange = method.Name.Returns(name);
            return method;
        }

        public static ParameterInfo ParameterInfo(string name) =>
            ParameterInfo(default, name);

        public static ParameterInfo ParameterInfo(Type? parameterType = default, string? name = default) {
            name = name ?? $"Parameter{Next}";
            parameterType = parameterType ?? Type();
            var parameter = Substitute.For<ParameterInfo>();
            arrange = parameter.Name.Returns(name);
            arrange = parameter.ParameterType.Returns(parameterType);
            return parameter;
        }

        public static PropertyInfo PropertyInfo(MethodAttributes attributes, Type? propertyType = default) {
            propertyType = propertyType ?? Type();
            var property = Substitute.For<PropertyInfo>();
            MethodInfo get = MethodInfo(attributes);
            arrange = property.GetMethod.Returns(get);
            arrange = property.PropertyType.Returns(propertyType);
            return property;
        }

        public static Type Type() {
            var type = Substitute.For<Type>();
            arrange = type.Name.Returns($"Type{Next}");
            arrange = type.Namespace.Returns($"Namespace{Next}");
            string fullName = $"{$"Namespace{Next}"}.{$"Type{Next}"}";
            arrange = type.FullName.Returns(fullName);
            return type;
        }

        public static T WithDeclaringType<T>(this T memberInfo, Type declaringType) where T : MemberInfo {
            arrange = memberInfo.DeclaringType.Returns(declaringType);
            return memberInfo;
        }
    }
}
