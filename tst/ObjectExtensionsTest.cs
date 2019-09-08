using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest
    {
        readonly object instance = new object();

        public class AccessibilityMethod: ObjectExtensionsTest
        {
            [Fact]
            public void InternalReturnsInternalMembersOfGivenInstance() {
                IMembers actual = instance.Internal();
                VerifyAccessibleMembers(instance, Accessibility.Internal, actual);
            }

            [Fact]
            public void PrivateReturnsPrivateMembersOfGivenInstance() {
                IMembers actual = instance.Private();
                VerifyAccessibleMembers(instance, Accessibility.Private, actual);
            }

            [Fact]
            public void ProtectedReturnsProtectedMembersOfGivenInstance() {
                IMembers actual = instance.Protected();
                VerifyAccessibleMembers(instance, Accessibility.Protected, actual);
            }

            [Fact]
            public void PublicReturnsPublicMembersOfGivenInstance() {
                IMembers actual = instance.Public();
                VerifyAccessibleMembers(instance, Accessibility.Public, actual);
            }

            static void VerifyAccessibleMembers(object instance, Accessibility accessibility, IMembers actual)
            {
                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(accessibility, accessibleMembers.Accessibility);
                VerifyInstanceMembers(instance, accessibleMembers.Source);
            }
        }

        public class ConstructorMethod: ConstructorExtensionsTest
        {
            readonly object instance = new object();

            [Fact]
            public void ReturnsSingleConstructorDeclaredByTypeOfGivenInstance() {
                Assert.Same(selected, instance.Constructor());
                var declared = Assert.IsType<DeclarationFilter<Constructor, ConstructorInfo>>(selection);
                Assert.Equal(instance.GetType(), declared.DeclaringType);
                VerifyInstanceConstructors(declared.Source, instance);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Assert.Same(selected, instance.Constructor(delegateType));
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                VerifyInstanceConstructors(filter.Source, instance);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = instance.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typed = VerifyFilter(selection, typeof(TestDelegate));
                VerifyInstanceConstructors(typed.Source, instance);
            }

            static void VerifyInstanceConstructors(IEnumerable<Constructor> filter, object instance) {
                var members = Assert.IsType<Members<ConstructorInfo, Constructor>>(filter);
                Assert.Same(instance, members.Instance);
            }
        }

        public class DeclaredMethod: ObjectExtensionsTest
        {
            [Fact]
            public void ReturnsInstanceMembersDeclaredByGivenType() {
                IMembers actual = instance.DeclaredBy(typeof(TestType));
                VerifyDeclaredMembers<TestType>(instance, actual);
            }

            [Fact]
            public void ReturnsInstanceMembersDeclaredByGivenGenericType() {
                IMembers actual = instance.DeclaredBy<TestType>();
                VerifyDeclaredMembers<TestType>(instance, actual);
            }

            [Fact]
            public void ReturnsInstanceMembersDeclaredByInstanceType() {
                var instance = new TestType();
                IMembers actual = instance.Declared();
                VerifyDeclaredMembers<TestType>(instance, actual);
            }

            static void VerifyDeclaredMembers<TDeclaringType>(object instance, IMembers actual) {
                var declaredMembers = Assert.IsType<DeclaredMembers>(actual);
                Assert.Equal(typeof(TDeclaringType), declaredMembers.DeclaringType);
                VerifyInstanceMembers(instance, declaredMembers.Source);
            }

            class TestType { }
        }

        public class EventMethod: EventExtensionsTest
        {
            [Fact]
            public void ReturnsSingleEventOfGivenInstance() {
                Assert.Same(selected, instance.Event());

                VerifyInstanceEvents(selection, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, instance.Event(eventName));

                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                VerifyInstanceEvents(named.Source, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, instance.Event(handlerType));

                EventTypeFilter named = VerifyFilter(selection, handlerType);
                VerifyInstanceEvents(named.Source, instance);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, instance.Event(handlerType, eventName));

                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                VerifyInstanceEvents(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = instance.Event<TestHandler>();

                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                VerifyInstanceEvents(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = instance.Event<TestHandler>(eventName);

                VerifyGenericEvent(selected, generic);
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                VerifyInstanceEvents(typed.Source, instance);
            }

            static void VerifyInstanceEvents(IEnumerable<Event> filter, object instance) {
                var events = Assert.IsType<Members<EventInfo, Event>>(filter);
                Assert.Same(instance, events.Instance);
            }
        }

        public class FieldMethod: FieldExtensionsTest
        {
            [Fact]
            public void ReturnsSingleFieldInGivenType() {
                Assert.Same(selected, instance.Field());

                VerifyInstanceFields(selection, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, instance.Field(fieldType));

                FieldTypeFilter named = VerifyFilter(selection, fieldType);
                VerifyInstanceFields(named.Source, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, instance.Field(fieldName));

                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                VerifyInstanceFields(named.Source, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, instance.Field(fieldType, fieldName));

                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Source, fieldType);
                VerifyInstanceFields(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericFieldOfGivenType() {
                Field<FieldValue> generic = instance.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typed = VerifyFilter(selection, typeof(FieldValue));
                VerifyInstanceFields(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = instance.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Source, typeof(FieldValue));
                VerifyInstanceFields(typed.Source, instance);
            }

            static void VerifyInstanceFields(IEnumerable<Field> filter, object instance) {
                var fields = Assert.IsType<Members<FieldInfo, Field>>(filter);
                Assert.Same(instance, fields.Instance);
            }
        }

        public class InheritanceMethod: ObjectExtensionsTest
        {
            class BaseType { }
            class TestType: BaseType { }

            new readonly object instance = new TestType();

            [Fact]
            public void InheritedReturnsInstanceMembersInheritedFromBaseType() {
                IMembers actual = instance.Inherited();
                VerifyInheritedMembers<BaseType>(instance, actual);
            }

            [Fact]
            public void InheritedFromReturnsInstanceMembersInheritedFromGivenType() {
                IMembers actual = instance.InheritedFrom(typeof(BaseType));
                VerifyInheritedMembers<BaseType>(instance, actual);
            }

            [Fact]
            public void GenericInheritedFromReturnsInstanceMembersInheritedFromGivenType() {
                IMembers actual = instance.InheritedFrom<BaseType>();
                VerifyInheritedMembers<BaseType>(instance, actual);
            }

            static void VerifyInheritedMembers<TAncestorType>(object instance, IMembers actual) {
                var inheritedMembers = Assert.IsType<InheritedMembers>(actual);
                Assert.Equal(typeof(TAncestorType), inheritedMembers.AncestorType);
                VerifyInstanceMembers(instance, inheritedMembers.Source);
            }
        }

        public class MethodMethod: MethodExtensionsTest
        {
            [Fact]
            public void ReturnsSingleMethodDeclaredByTypeOfGivenInstance() {
                Assert.Same(selected, instance.Method());
                var declared = Assert.IsType<DeclarationFilter<Method, MethodInfo>>(selection);
                Assert.Equal(instance.GetType(), declared.DeclaringType);
                VerifyInstanceMethods(declared.Source, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenType() {
                Assert.Same(selected, instance.Method(methodType));

                MethodTypeFilter named = VerifyFilter(selection, methodType);
                VerifyInstanceMethods(named.Source, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, instance.Method(methodName));

                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                VerifyInstanceMethods(named.Source, instance);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, instance.Method(methodType, methodName));

                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Source, methodType);
                VerifyInstanceMethods(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericMethodOfGivenType() {
                Method<MethodType> generic = instance.Method<MethodType>();

                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typed = VerifyFilter(selection, typeof(MethodType));
                VerifyInstanceMethods(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = instance.Method<MethodType>(methodName);

                VerifyGenericMethod(selected, generic);
                MemberNameFilter<Method, MethodInfo> named = VerifyFilter(selection, methodName);
                MethodTypeFilter typed = VerifyFilter(named.Source, typeof(MethodType));
                VerifyInstanceMethods(typed.Source, instance);
            }

            static void VerifyInstanceMethods(IEnumerable<Method> source, object instance) {
                var members = Assert.IsType<Members<MethodInfo, Method>>(source);
                Assert.Same(instance, members.Instance);
            }
        }

        public class ObjectExtension: PropertyExtensionsTest
        {
            [Fact]
            public void ReturnsSinglePropertyInGivenType() {
                Assert.Same(selected, instance.Property());

                VerifyInstanceProperties(selection, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenName() {
                Assert.Same(selected, instance.Property(propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                VerifyInstanceProperties(named.Source, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenType() {
                Assert.Same(selected, instance.Property(propertyType));

                PropertyTypeFilter named = VerifyFilter(selection, propertyType);
                VerifyInstanceProperties(named.Source, instance);
            }

            [Fact]
            public void ReturnsPropertyWithGivenTypeAndName() {
                Assert.Same(selected, instance.Property(propertyType, propertyName));

                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Source, propertyType);
                VerifyInstanceProperties(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericPropertyOfGivenType() {
                Property<PropertyValue> generic = instance.Property<PropertyValue>();

                VerifyGenericProperty(selected, generic);
                PropertyTypeFilter typed = VerifyFilter(selection, typeof(PropertyValue));
                VerifyInstanceProperties(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenTypeAndName() {
                Property<PropertyValue> generic = instance.Property<PropertyValue>(propertyName);

                VerifyGenericProperty(selected, generic);
                MemberNameFilter<Property, PropertyInfo> named = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typed = VerifyFilter(named.Source, typeof(PropertyValue));
                VerifyInstanceProperties(typed.Source, instance);
            }

            static void VerifyInstanceProperties(IEnumerable<Property> filter, object instance) {
                var properties = Assert.IsType<Members<PropertyInfo, Property>>(filter);
                Assert.Same(instance, properties.Instance);
            }
        }

        internal static void VerifyInstanceMembers(object instance, IMembers actual) {
            var instanceMembers = Assert.IsType<InstanceMembers>(actual);
            Assert.Same(instance, instanceMembers.Instance);
        }
    }
}
