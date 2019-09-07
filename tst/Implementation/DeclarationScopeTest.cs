using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class DeclarationScopeTest
    {
        readonly IScope sut;

        // Constructor parameters
        readonly IScope previous = Substitute.For<IScope>();
        readonly Type declaringType = Type();

        public DeclarationScopeTest() =>
            sut = new DeclarationScope(previous, declaringType);

        public class Ctor: DeclarationScopeTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousScopeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new DeclarationScope(null, declaringType));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDeclaringTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new DeclarationScope(previous, null));
                Assert.Equal("declaringType", thrown.ParamName);
            }
        }

        public class Previous: DeclarationScopeTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() =>
                Assert.Same(previous, ((IDecorator<IScope>)sut).Previous);
        }

        public class DeclaringType: DeclarationScopeTest
        {
            [Fact]
            public void IsAssignedByConstructor() =>
                Assert.Same(declaringType, ((DeclarationScope)sut).DeclaringType);
        }

        public class Constructors: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsConstructorsWithMatchingDeclaringType() {
                var allConstructors = Substitute.For<IEnumerable<Constructor>>();
                ConfiguredCall arrange = previous.Constructors().Returns(allConstructors);

                IEnumerable<Constructor> actual = sut.Constructors();

                var declaredConstructors = Assert.IsType<DeclaredMembers<Constructor, ConstructorInfo>>(actual);
                Assert.Equal(declaringType, declaredConstructors.DeclaringType);
                Assert.Same(allConstructors, declaredConstructors.Previous);
            }
        }

        public class Events: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsEventsWithMatchingDeclaringType() {
                var allEvents = Substitute.For<IEnumerable<Event>>();
                ConfiguredCall arrange = previous.Events().Returns(allEvents);

                IEnumerable<Event> actual = sut.Events();

                var declaredEvents = Assert.IsType<DeclaredMembers<Event, EventInfo>>(actual);
                Assert.Equal(declaringType, declaredEvents.DeclaringType);
                Assert.Same(allEvents, declaredEvents.Previous);
            }
        }

        public class Fields: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsFieldsWithMatchingDeclaringType() {
                var allFields = Substitute.For<IEnumerable<Field>>();
                ConfiguredCall arrange = previous.Fields().Returns(allFields);

                IEnumerable<Field> actual = sut.Fields();

                var declaredFields = Assert.IsType<DeclaredMembers<Field, FieldInfo>>(actual);
                Assert.Equal(declaringType, declaredFields.DeclaringType);
                Assert.Same(allFields, declaredFields.Previous);
            }
        }

        public class Methods: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsMethodsWithMatchingDeclaringType() {
                var allMethods = Substitute.For<IEnumerable<Method>>();
                ConfiguredCall arrange = previous.Methods().Returns(allMethods);

                IEnumerable<Method> actual = sut.Methods();

                var declaredMethods = Assert.IsType<DeclaredMembers<Method, MethodInfo>>(actual);
                Assert.Equal(declaringType, declaredMethods.DeclaringType);
                Assert.Same(allMethods, declaredMethods.Previous);
            }
        }

        public class Properties: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsPropertiesWithMatchingDeclaringType() {
                var allProperties = Substitute.For<IEnumerable<Property>>();
                ConfiguredCall arrange = previous.Properties().Returns(allProperties);

                IEnumerable<Property> actual = sut.Properties();

                var declaredProperties = Assert.IsType<DeclaredMembers<Property, PropertyInfo>>(actual);
                Assert.Equal(declaringType, declaredProperties.DeclaringType);
                Assert.Same(allProperties, declaredProperties.Previous);
            }
        }
    }
}
