using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class TypeExtensionsTest
    {
        class TestClass { }

        public class AccessibilityMethod: TypeExtensionsTest
        {
            readonly Type type = typeof(TestClass);

            [Fact]
            public void InternalReturnsInternalMembersOfGivenType() {
                IMembers actual = type.Internal();
                VerifyMembers(actual, type, Accessibility.Internal);
            }

            [Fact]
            public void PrivateReturnsPrivateMembersOfGivenType() {
                IMembers actual = type.Private();
                VerifyMembers(actual, type, Accessibility.Private);
            }

            [Fact]
            public void ProtectedReturnsProtectedMembersOfGivenType() {
                IMembers actual = type.Protected();
                VerifyMembers(actual, type, Accessibility.Protected);
            }

            [Fact]
            public void PublicReturnsPublicMembersOfGivenType() {
                IMembers actual = type.Public();
                VerifyMembers(actual, type, Accessibility.Public);
            }

            static void VerifyMembers(IMembers actual, Type type, Accessibility accessibility) {
                var accessibleMembers = (AccessibleMembers)actual;
                Assert.Equal(accessibility, accessibleMembers.Accessibility);

                var staticMembers = (StaticMembers)accessibleMembers.Source;
                Assert.Same(type, staticMembers.Type);
            }
        }

        public class ConstructorMethod: ConstructorExtensionsTest
        {
            readonly Type type = typeof(TestClass);

            [Fact]
            public void ReturnsSingleConstructorOfGivenType() {
                Assert.Same(selected, type.Constructor());
                Assert.NotNull(selection);
                var members = (Members<ConstructorInfo, Constructor>)selection!;
                Assert.Same(type, members.Type);
            }
        }

        public class DeclaredMethod: TypeExtensionsTest
        {
            // Method parameters
            readonly Type type = Type();

            [Fact]
            public void ReturnsStaticMembersDeclaredByGivenType() {
                IMembers actual = type.DeclaredBy(typeof(TestClass));
                VerifyDeclaredMembers<TestClass>(type, actual);
            }

            [Fact]
            public void ReturnsStaticMembersDeclaredByGivenGenericType() {
                IMembers actual = type.DeclaredBy<TestClass>();
                VerifyDeclaredMembers<TestClass>(type, actual);
            }

            [Fact]
            public void ReturnsStaticMembersDeclaredByTypeItself() {
                IMembers actual = typeof(TestClass).Declared();
                VerifyDeclaredMembers<TestClass>(typeof(TestClass), actual);
            }

            static void VerifyDeclaredMembers<TDeclaringType>(Type staticType, IMembers actual) {
                var declaredMembers = (DeclaredMembers)actual;
                Assert.Equal(typeof(TDeclaringType), declaredMembers.DeclaringType);
                var staticMembers = (StaticMembers)declaredMembers.Source;
                Assert.Equal(staticType, staticMembers.Type);
            }
        }

        public class EventMethod: EventExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSingleEventInGivenType() {
                Assert.Same(selected, testType.Event());

                VerifyStaticEvents(selection, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, testType.Event(eventName));

                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                VerifyStaticEvents(named.Source, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, testType.Event(handlerType));

                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyStaticEvents(typed.Source, testType);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, testType.Event(handlerType, eventName));

                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                VerifyStaticEvents(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = testType.Event<TestHandler>();

                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyStaticEvents(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = testType.Event<TestHandler>(eventName);

                VerifyGenericEvent(selected, generic);
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                VerifyStaticEvents(typed.Source, testType);
            }

            static void VerifyStaticEvents(IEnumerable<Event>? selection, Type type) {
                Assert.NotNull(selection);
                var events = (Members<EventInfo, Event>)selection!;
                Assert.Same(type, events.Type);
            }
        }

        public class FieldMethod: FieldExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSingleFieldInGivenType() {
                Assert.Same(selected, testType.Field());

                VerifyStaticFields(selection, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, testType.Field(fieldType));

                FieldTypeFilter typed = VerifyFilter(selection, fieldType);
                VerifyStaticFields(typed.Source, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, testType.Field(fieldName));

                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                VerifyStaticFields(named.Source, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, testType.Field(fieldType, fieldName));

                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Source, fieldType);
                VerifyStaticFields(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericFieldOfGivenType() {
                Field<FieldValue> generic = testType.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typed = VerifyFilter(selection, typeof(FieldValue));
                VerifyStaticFields(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = testType.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Source, typeof(FieldValue));
                VerifyStaticFields(typed.Source, testType);
            }

            static void VerifyStaticFields(IEnumerable<Field>? selection, Type expected) {
                Assert.NotNull(selection);
                var fields = (Members<FieldInfo, Field>)selection!;
                Assert.Same(expected, fields.Type);
            }
        }

        public class InheritanceMethod: TypeExtensionsTest
        {
            class DerivedClass: TestClass { }

            // Method parameters
            readonly Type type = typeof(DerivedClass);

            [Fact]
            public void InheritedReturnsStaticMembersInheritedFromBaseType() {
                IMembers actual = typeof(DerivedClass).Inherited();
                VerifyInheritedMembers<TestClass>(typeof(DerivedClass), actual);
            }

            [Fact]
            public void InheritedFromReturnsStaticMembersInheritedFromGivenType() {
                IMembers actual = type.InheritedFrom(typeof(TestClass));
                VerifyInheritedMembers<TestClass>(type, actual);
            }

            [Fact]
            public void GenericInheritedFromReturnsStaticMembersInheritedFromGivenType() {
                IMembers actual = type.InheritedFrom<TestClass>();
                VerifyInheritedMembers<TestClass>(type, actual);
            }

            static void VerifyInheritedMembers<TAncestorType>(Type staticType, IMembers actual) {
                var inheritedMembers = (InheritedMembers)actual;
                Assert.Equal(typeof(TAncestorType), inheritedMembers.AncestorType);
                var staticMembers = (StaticMembers)inheritedMembers.Source;
                Assert.Equal(staticType, staticMembers.Type);
            }
        }

        public class MethodMethod: MethodExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSingleMethodInGivenType() {
                Assert.Same(selected, testType.Method());

                VerifyStaticMethods(selection, testType);
            }

            [Fact]
            public void ReturnsMethodWithGivenType() {
                Assert.Same(selected, testType.Method(methodType));

                MethodTypeFilter typed = VerifyFilter(selection, methodType);
                VerifyStaticMethods(typed.Source, testType);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, testType.Method(methodName));

                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                VerifyStaticMethods(named.Source, testType);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, testType.Method(methodType, methodName));

                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Source, methodType);
                VerifyStaticMethods(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericMethodOfGivenType() {
                Method<MethodType> generic = testType.Method<MethodType>();

                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typed = VerifyFilter(selection, typeof(MethodType));
                VerifyStaticMethods(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = testType.Method<MethodType>(methodName);

                VerifyGenericMethod(selected, generic);
                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Source, typeof(MethodType));
                VerifyStaticMethods(typed.Source, testType);
            }

            static void VerifyStaticMethods(IEnumerable<Method>? selection, Type expected) {
                Assert.NotNull(selection);
                var methods = (Members<MethodInfo, Method>)selection!;
                Assert.Same(expected, methods.Type);
            }
        }

        public class New: TypeExtensionsTest
        {
            class PropertyType { }

            class TypeWithPublicConstructor
            {
                public TypeWithPublicConstructor(PropertyType value) =>
                    Property = value;

                public PropertyType Property { get; }
            }

            [Fact]
            public void ReturnsNewInstanceCreatedByPublicConstructor() {
                var value = new PropertyType();
                var instance = (TypeWithPublicConstructor)typeof(TypeWithPublicConstructor).New(value);
                Assert.Same(value, instance.Property);
            }

            class TypeWithPrivateConstructor
            {
                TypeWithPrivateConstructor(PropertyType value) =>
                    Property = value;

                public PropertyType Property { get; }
            }

            [Fact]
            public void ReturnsNewInstanceCreatedByPrivateConstructor() {
                var value = new PropertyType();
                var instance = (TypeWithPrivateConstructor)typeof(TypeWithPrivateConstructor).New(value);
                Assert.Same(value, instance.Property);
            }

            class TestException: Exception { }

            class TypeWithThrowingConstructor
            {
                TypeWithThrowingConstructor() => throw new TestException();
            }

            [Fact]
            public void UnwrapsOriginalExceptionThrownByConstructor() =>
                Assert.Throws<TestException>(() => typeof(TypeWithThrowingConstructor).New());
        }

        public class PropertyMethod: PropertyExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSinglePropertyInGivenType() {
                Assert.Same(selected, testType.Property());

                VerifyStaticProperties(selection, testType);
            }

            [Fact]
            public void ReturnsPropertyWithGivenName() {
                Assert.Same(selected, testType.Property(propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                VerifyStaticProperties(named.Source, testType);
            }

            [Fact]
            public void ReturnsPropertyWithGivenType() {
                Assert.Same(selected, testType.Property(propertyType));

                PropertyTypeFilter typed = VerifyFilter(selection, propertyType);
                VerifyStaticProperties(typed.Source, testType);
            }

            [Fact]
            public void ReturnsPropertyWithGivenTypeAndName() {
                Assert.Same(selected, testType.Property(propertyType, propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Source, propertyType);
                VerifyStaticProperties(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericPropertyOfGivenType() {
                Property<PropertyValue> generic = testType.Property<PropertyValue>();

                VerifyGenericProperty(selected, generic);
                PropertyTypeFilter typed = VerifyFilter(selection, typeof(PropertyValue));
                VerifyStaticProperties(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenTypeAndName() {
                Property<PropertyValue> generic = testType.Property<PropertyValue>(propertyName);

                VerifyGenericProperty(selected, generic);
                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Source, typeof(PropertyValue));
                VerifyStaticProperties(typed.Source, testType);
            }

            static void VerifyStaticProperties(IEnumerable<Property>? selection, Type expected) {
                Assert.NotNull(selection);
                var properties = (Members<PropertyInfo, Property>)selection!;
                Assert.Same(expected, properties.Type);
            }
        }

        public class Uninitialized: TypeExtensionsTest
        {
            class TestType
            {
                public readonly int TestField = 42;
            }

            [Fact]
            public void ReturnsUninitializedInstanceOfGivenType() {
                var instance = (TestType)typeof(TestType).Uninitialized();
                Assert.Equal(0, instance.TestField);
            }
        }
    }
}
