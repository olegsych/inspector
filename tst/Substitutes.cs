using System;
using System.Reflection;
using NSubstitute;

namespace Inspector
{
    static class Substitutes
    {
        static uint seed = 1;

        static uint Next => seed++;

        public static ConstructorInfo ConstructorInfo(MethodAttributes attributes) {
            var constructor = Substitute.For<ConstructorInfo>();
            constructor.Attributes.Returns(attributes);
            return constructor;
        }

        public static EventInfo EventInfo(MethodAttributes attributes, string eventName) =>
            EventInfo(attributes, null, eventName);

        public static EventInfo EventInfo(MethodAttributes attributes, Type handlerType = default, string eventName = default) {
            handlerType = handlerType ?? Type();
            eventName = eventName ?? $"Event{Next}";
            var @event = Substitute.For<EventInfo>();
            var addMethod = MethodInfo(attributes);
            @event.AddMethod.Returns(addMethod);
            @event.EventHandlerType.Returns(handlerType);
            @event.Name.Returns(eventName);
            return @event;
        }

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

        public static MethodInfo MethodInfo(MethodAttributes attributes, string name = default) {
            name = name ?? $"Method{Next}";
            var method = Substitute.For<MethodInfo>();
            method.Attributes.Returns(attributes);
            method.Name.Returns(name);
            return method;
        }

        public static ParameterInfo ParameterInfo(Type parameterType = default) {
            parameterType = parameterType ?? Type();
            var parameter = Substitute.For<ParameterInfo>();
            parameter.ParameterType.Returns(parameterType);
            return parameter;
        }

        public static PropertyInfo PropertyInfo(MethodAttributes attributes, Type propertyType = default) {
            propertyType = propertyType ?? Type();
            var property = Substitute.For<PropertyInfo>();
            MethodInfo get = MethodInfo(attributes);
            property.GetMethod.Returns(get);
            property.PropertyType.Returns(propertyType);
            return property;
        }

        public static Type Type() {
            var type = Substitute.For<Type>();
            type.Name.Returns($"Type{Next}");
            type.Namespace.Returns($"Namespace{Next}");
            string fullName = $"{$"Namespace{Next}"}.{$"Type{Next}"}";
            type.FullName.Returns(fullName);
            return type;
        }

        public static T WithDeclaringType<T>(this T memberInfo, Type declaringType) where T : MemberInfo {
            memberInfo.DeclaringType.Returns(declaringType);
            return memberInfo;
        }
    }
}
