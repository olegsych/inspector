using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class IMembersExtensionsTest
    {
        readonly IMembers members = Substitute.For<IMembers>();

        public class ConstructorMethod: ConstructorExtensionsTest
        {
            readonly IMembers members = Substitute.For<IMembers>();

            // Test fixture
            readonly IEnumerable<Constructor> constructors = Substitute.For<IEnumerable<Constructor>>();

            public ConstructorMethod() =>
                members.Constructors().Returns(constructors);

            [Fact]
            public void ReturnsSingleConstructor() {
                Constructor actual = members.Constructor();

                Assert.Same(selected, actual);
                Assert.Same(constructors, selection);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Constructor actual = members.Constructor(delegateType);

                Assert.Same(selected, actual);
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                Assert.Same(constructors, filter.Source);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = members.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typeFilter = VerifyFilter(selection, typeof(TestDelegate));
                Assert.Same(constructors, typeFilter.Source);
            }
        }

        [Fact]
        public void DeclaredByReturnsMembersDeclaredByGivenType() {
            IMembers actual = members.DeclaredBy(typeof(TestType));

            VerifyDeclaredMembers<TestType>(members, actual);
        }

        [Fact]
        public void DeclaredByGenericReturnsMembersDeclaredByGivenType() {
            IMembers actual = members.DeclaredBy<TestType>();

            VerifyDeclaredMembers<TestType>(members, actual);
        }

        public class EventMethod: EventExtensionsTest
        {
            readonly IMembers members = Substitute.For<IMembers>();

            // Test fixture
            readonly IEnumerable<Event> events = Substitute.For<IEnumerable<Event>>();

            public EventMethod() =>
                members.Events().Returns(events);

            [Fact]
            public void ReturnsSingleEvent() {
                Assert.Same(selected, members.Event());
                Assert.Same(events, selection);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, members.Event(eventName));
                MemberNameFilter<Event, EventInfo> filter = VerifyFilter(selection, eventName);
                Assert.Same(events, filter.Source);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, members.Event(handlerType));
                EventTypeFilter filter = VerifyFilter(selection, handlerType);
                Assert.Same(events, filter.Source);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, members.Event(handlerType, eventName));
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                Assert.Same(events, typed.Source);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = members.Event<TestHandler>();
                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                Assert.Same(events, typed.Source);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = members.Event<TestHandler>(eventName);
                VerifyGenericEvent(selected, generic);
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                Assert.Same(events, typed.Source);
            }
        }

        public class FieldMethod: FieldExtensionsTest
        {
            readonly IMembers members = Substitute.For<IMembers>();

            // Arrange
            readonly IEnumerable<Field> fields = Substitute.For<IEnumerable<Field>>();

            public FieldMethod() =>
                members.Fields().Returns(fields);

            [Fact]
            public void ReturnsSingleField() {
                Assert.Same(selected, members.Field());
                Assert.Same(fields, selection);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, members.Field(fieldName));
                MemberNameFilter<Field, FieldInfo> filter = VerifyFilter(selection, fieldName);
                Assert.Same(fields, filter.Source);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, members.Field(fieldType));
                FieldTypeFilter filter = VerifyFilter(selection, fieldType);
                Assert.Same(fields, filter.Source);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, members.Field(fieldType, fieldName));
                MemberNameFilter<Field, FieldInfo> nameFilter = VerifyFilter(selection, fieldName);
                FieldTypeFilter typeFilter = VerifyFilter(nameFilter.Source, fieldType);
                Assert.Same(fields, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenType() {
                Field<FieldValue> generic = members.Field<FieldValue>();
                VerifyGenericField(selected, generic);
                FieldTypeFilter typeFilter = VerifyFilter(selection, typeof(FieldValue));
                Assert.Same(fields, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = members.Field<FieldValue>(fieldName);
                VerifyGenericField(selected, generic);
                MemberNameFilter<Field, FieldInfo> nameFilter = VerifyFilter(selection, fieldName);
                FieldTypeFilter typeFilter = VerifyFilter(nameFilter.Source, fieldType);
                Assert.Same(fields, typeFilter.Source);
            }
        }

        [Fact]
        public void InternalReturnsInternalMembers() {
            IMembers actual = members.Internal();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Internal, accessibleMembers.Accessibility);
        }

        [Fact]
        public void InheritedFromReturnsMembersInheritedFromGivenType() {
            IMembers actual = members.InheritedFrom(typeof(TestType));

            VerifyInheritedMembers<TestType>(members, actual);
        }

        [Fact]
        public void InheritedFromGenericReturnsMembersInheritedFromGivenType() {
            IMembers actual = members.InheritedFrom<TestType>();

            VerifyInheritedMembers<TestType>(members, actual);
        }

        public class MethodTest: MethodExtensionsTest
        {
            // Method parameters
            readonly IMembers members = Substitute.For<IMembers>();

            // Arrange
            readonly IEnumerable<Method> methods = Substitute.For<IEnumerable<Method>>();

            public MethodTest() =>
                members.Methods().Returns(methods);

            [Fact]
            public void ReturnsSingleMethod() {
                Assert.Same(selected, members.Method());
                Assert.Same(methods, selection);
            }

            [Fact]
            public void ReturnsMethodWithGivenName() {
                Assert.Same(selected, members.Method(methodName));
                MemberNameFilter<Method, MethodInfo> filter = VerifyFilter(selection, methodName);
                Assert.Same(methods, filter.Source);
            }

            [Fact]
            public void ReturnsMethodWithGivenType() {
                Assert.Same(selected, members.Method(methodType));
                MethodTypeFilter filter = VerifyFilter(selection, methodType);
                Assert.Same(methods, filter.Source);
            }

            [Fact]
            public void ReturnsMethodWithGivenTypeAndName() {
                Assert.Same(selected, members.Method(methodType, methodName));
                MemberNameFilter<Method, MethodInfo> nameFilter = VerifyFilter(selection, methodName);
                MethodTypeFilter typeFilter = VerifyFilter(nameFilter.Source, methodType);
                Assert.Same(methods, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenType() {
                Method<MethodType> generic = members.Method<MethodType>();
                VerifyGenericMethod(selected, generic);
                MethodTypeFilter typeFilter = VerifyFilter(selection, typeof(MethodType));
                Assert.Same(methods, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericMethodWithGivenTypeAndName() {
                Method<MethodType> generic = members.Method<MethodType>(methodName);
                VerifyGenericMethod(selected, generic);
                MemberNameFilter<Method, MethodInfo> nameFilter = VerifyFilter(selection, methodName);
                MethodTypeFilter typeFilter = VerifyFilter(nameFilter.Source, methodType);
                Assert.Same(methods, typeFilter.Source);
            }
        }

        [Fact]
        public void PrivateReturnsPrivateMembers() {
            IMembers actual = members.Private();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Private, accessibleMembers.Accessibility);
        }

        public class PropertyMethod: PropertyExtensionsTest
        {
            readonly IMembers members = Substitute.For<IMembers>();

            // Arrange
            readonly IEnumerable<Property> properties = Substitute.For<IEnumerable<Property>>();

            public PropertyMethod() =>
                members.Properties().Returns(properties);

            [Fact]
            public void ReturnsSingleProperty() {
                Assert.Same(selected, members.Property());
                Assert.Same(properties, selection);
            }

            [Fact]
            public void ReturnsPropertyWithGivenName() {
                Assert.Same(selected, members.Property(propertyName));
                MemberNameFilter<Property, PropertyInfo> filter = VerifyFilter(selection, propertyName);
                Assert.Same(properties, filter.Source);
            }

            [Fact]
            public void ReturnsPropertyWithGivenType() {
                Assert.Same(selected, members.Property(propertyType));
                PropertyTypeFilter filter = VerifyFilter(selection, propertyType);
                Assert.Same(properties, filter.Source);
            }

            [Fact]
            public void ReturnsPropertyWithGivenTypeAndName() {
                Assert.Same(selected, members.Property(propertyType, propertyName));
                MemberNameFilter<Property, PropertyInfo> nameFilter = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typeFilter = VerifyFilter(nameFilter.Source, propertyType);
                Assert.Same(properties, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenType() {
                Property<PropertyValue> generic = members.Property<PropertyValue>();
                VerifyGenericProperty(selected, generic);
                PropertyTypeFilter typeFilter = VerifyFilter(selection, typeof(PropertyValue));
                Assert.Same(properties, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericPropertyWithGivenTypeAndName() {
                Property<PropertyValue> generic = members.Property<PropertyValue>(propertyName);
                VerifyGenericProperty(selected, generic);
                MemberNameFilter<Property, PropertyInfo> nameFilter = VerifyFilter(selection, propertyName);
                PropertyTypeFilter typeFilter = VerifyFilter(nameFilter.Source, propertyType);
                Assert.Same(properties, typeFilter.Source);
            }
        }

        [Fact]
        public void ProtectedReturnsProtectedMembers() {
            IMembers actual = members.Protected();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Protected, accessibleMembers.Accessibility);
        }

        [Fact]
        public void PublicReturnsPublicMembers() {
            IMembers actual = members.Public();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Public, accessibleMembers.Accessibility);
        }

        static void VerifyDeclaredMembers<TDeclaringType>(IMembers source, IMembers actual) {
            var declaredMembers = Assert.IsType<DeclaredMembers>(actual);
            Assert.Equal(typeof(TDeclaringType), declaredMembers.DeclaringType);
            Assert.Same(source, declaredMembers.Source);
        }

        static void VerifyInheritedMembers<TAncestorType>(IMembers source, IMembers actual) {
            var inheritedMembers = Assert.IsType<InheritedMembers>(actual);
            Assert.Equal(typeof(TAncestorType), inheritedMembers.AncestorType);
            Assert.Same(source, inheritedMembers.Source);
        }

        class TestType {}
    }
}
