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
        readonly IScope source = Substitute.For<IScope>();
        readonly Type declaringType = Type();

        public DeclarationScopeTest() =>
            sut = new DeclarationScope(source, declaringType);

        public class Ctor: DeclarationScopeTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenSourceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new DeclarationScope(null, declaringType));
                Assert.Equal("source", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDeclaringTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new DeclarationScope(source, null));
                Assert.Equal("declaringType", thrown.ParamName);
            }
        }

        public class Source: DeclarationScopeTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() =>
                Assert.Same(source, ((IDecorator<IScope>)sut).Source);
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
                ConfiguredCall arrange = source.Constructors().Returns(allConstructors);

                IEnumerable<Constructor> actual = sut.Constructors();

                var declaredConstructors = Assert.IsType<DeclaredMembers<Constructor, ConstructorInfo>>(actual);
                Assert.Equal(declaringType, declaredConstructors.DeclaringType);
                Assert.Same(allConstructors, declaredConstructors.Source);
            }
        }

        public class Events: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsEventsWithMatchingDeclaringType() {
                var allEvents = Substitute.For<IEnumerable<Event>>();
                ConfiguredCall arrange = source.Events().Returns(allEvents);

                IEnumerable<Event> actual = sut.Events();

                var declaredEvents = Assert.IsType<DeclaredMembers<Event, EventInfo>>(actual);
                Assert.Equal(declaringType, declaredEvents.DeclaringType);
                Assert.Same(allEvents, declaredEvents.Source);
            }
        }

        public class Fields: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsFieldsWithMatchingDeclaringType() {
                var allFields = Substitute.For<IEnumerable<Field>>();
                ConfiguredCall arrange = source.Fields().Returns(allFields);

                IEnumerable<Field> actual = sut.Fields();

                var declaredFields = Assert.IsType<DeclaredMembers<Field, FieldInfo>>(actual);
                Assert.Equal(declaringType, declaredFields.DeclaringType);
                Assert.Same(allFields, declaredFields.Source);
            }
        }

        public class Methods: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsMethodsWithMatchingDeclaringType() {
                var allMethods = Substitute.For<IEnumerable<Method>>();
                ConfiguredCall arrange = source.Methods().Returns(allMethods);

                IEnumerable<Method> actual = sut.Methods();

                var declaredMethods = Assert.IsType<DeclaredMembers<Method, MethodInfo>>(actual);
                Assert.Equal(declaringType, declaredMethods.DeclaringType);
                Assert.Same(allMethods, declaredMethods.Source);
            }
        }

        public class Properties: DeclarationScopeTest
        {
            [Fact]
            public void ReturnsPropertiesWithMatchingDeclaringType() {
                var allProperties = Substitute.For<IEnumerable<Property>>();
                ConfiguredCall arrange = source.Properties().Returns(allProperties);

                IEnumerable<Property> actual = sut.Properties();

                var declaredProperties = Assert.IsType<DeclaredMembers<Property, PropertyInfo>>(actual);
                Assert.Equal(declaringType, declaredProperties.DeclaringType);
                Assert.Same(allProperties, declaredProperties.Source);
            }
        }
    }
}
