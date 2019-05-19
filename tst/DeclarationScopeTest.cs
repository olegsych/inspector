using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
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

        public class GetConstructors: DeclarationScopeTest
        {
            new readonly IFilter<Constructor> sut;
            new readonly IFilter<Constructor> previous;

            public GetConstructors() {
                sut = base.sut;
                previous = base.previous;
            }

            [Fact]
            public void ReturnsConstructorsWithMatchingDeclaringType() {
                // Arrange
                ConstructorInfo constructorInfo = ConstructorInfo(MethodAttributes.Static).WithDeclaringType(declaringType);

                var expected = new[] { new Constructor(constructorInfo), new Constructor(constructorInfo) };

                var mixed = new[] {
                    new Constructor(ConstructorInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                    expected[0],
                    new Constructor(ConstructorInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                    expected[1],
                    new Constructor(ConstructorInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Constructor> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }

        public class GetEvents: DeclarationScopeTest
        {
            new readonly IFilter<Event> sut;
            new readonly IFilter<Event> previous;

            public GetEvents() {
                sut = base.sut;
                previous = base.previous;
            }

            [Fact]
            public void ReturnsEventsWithMatchingDeclaringType() {
                // Arrange
                EventInfo eventInfo = EventInfo(MethodAttributes.Static).WithDeclaringType(declaringType);

                var expected = new[] { new Event(eventInfo), new Event(eventInfo) };

                var mixed = new[] {
                    new Event(EventInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                    expected[0],
                    new Event(EventInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                    expected[1],
                    new Event(EventInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Event> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }

        public class GetFields: DeclarationScopeTest
        {
            new readonly IFilter<Field> sut;
            new readonly IFilter<Field> previous;

            public GetFields() {
                sut = base.sut;
                previous = base.previous;
            }

            [Fact]
            public void ReturnsFieldsWithMatchingDeclaringType() {
                // Arrange
                FieldInfo fieldInfo = FieldInfo(FieldAttributes.Static).WithDeclaringType(declaringType);

                var expected = new[] { new Field(fieldInfo), new Field(fieldInfo) };

                var mixed = new[] {
                    new Field(FieldInfo(FieldAttributes.Static).WithDeclaringType(Type())),
                    expected[0],
                    new Field(FieldInfo(FieldAttributes.Static).WithDeclaringType(Type())),
                    expected[1],
                    new Field(FieldInfo(FieldAttributes.Static).WithDeclaringType(Type())),
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Field> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }

        public class GetMethods: DeclarationScopeTest
        {
            new readonly IFilter<Method> sut;
            new readonly IFilter<Method> previous;

            public GetMethods() {
                sut = base.sut;
                previous = base.previous;
            }

            [Fact]
            public void ReturnsMethodsWithMatchingDeclaringType() {
                // Arrange
                MethodInfo methodInfo = MethodInfo(MethodAttributes.Static).WithDeclaringType(declaringType);

                var expected = new[] { new Method(methodInfo), new Method(methodInfo) };

                var mixed = new[] {
                    new Method(MethodInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                    expected[0],
                    new Method(MethodInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                    expected[1],
                    new Method(MethodInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Method> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }

        public class GetProperties: DeclarationScopeTest
        {
            new readonly IFilter<Property> sut;
            new readonly IFilter<Property> previous;

            public GetProperties() {
                sut = base.sut;
                previous = base.previous;
            }

            [Fact]
            public void ReturnsPropertiesWithMatchingDeclaringType() {
                // Arrange
                PropertyInfo propertyInfo = PropertyInfo(MethodAttributes.Static).WithDeclaringType(declaringType);

                var expected = new[] { new Property(propertyInfo), new Property(propertyInfo) };

                var mixed = new[] {
                    new Property(PropertyInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                    expected[0],
                    new Property(PropertyInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                    expected[1],
                    new Property(PropertyInfo(MethodAttributes.Static).WithDeclaringType(Type())),
                };

                previous.Get().Returns(mixed);

                // Act
                IEnumerable<Property> actual = sut.Get();

                Assert.Equal(expected, actual);
            }
        }
    }
}
