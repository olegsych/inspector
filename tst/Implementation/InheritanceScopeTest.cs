using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class InheritanceScopeTest
    {
        readonly IScope sut;

        // Constructor parameters
        readonly IScope previous = Substitute.For<IScope>();
        readonly Type ancestor = typeof(Parent);

        // Test fixture
        public InheritanceScopeTest() =>
            sut = new InheritanceScope(previous, ancestor);

        readonly Type grandParent = typeof(GrandParent);
        readonly Type parent = typeof(Parent);
        readonly Type child = typeof(Child);

        public class Ctor: InheritanceScopeTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousScopeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new InheritanceScope(null, ancestor));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenAncestorTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new InheritanceScope(previous, null));
                Assert.Equal("ancestorType", thrown.ParamName);
            }
        }

        public class Previous: InheritanceScopeTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsValueGivenToConstructor() =>
                Assert.Same(previous, ((IDecorator<IScope>)sut).Previous);
        }

        public class AncestorType: InheritanceScopeTest
        {
            [Fact]
            public void IsAssignedByConstructor() =>
                Assert.Same(ancestor, ((InheritanceScope)sut).AncestorType);
        }

        public class Constructors: InheritanceScopeTest
        {
            [Fact]
            public void ReturnsConstructorsInheritedFromAncestor() {
                // Arrange
                var expected = new Constructor[] {
                    new Constructor(ConstructorInfo(MethodAttributes.Static).WithDeclaringType(parent)),
                    new Constructor(ConstructorInfo(MethodAttributes.Static).WithDeclaringType(grandParent))
                };

                var mixed = new Constructor[] {
                    new Constructor(ConstructorInfo(MethodAttributes.Static).WithDeclaringType(child)),
                    expected[0],
                    new Constructor(ConstructorInfo(MethodAttributes.Static).WithDeclaringType(child)),
                    expected[1],
                    new Constructor(ConstructorInfo(MethodAttributes.Static).WithDeclaringType(child))
                };

                previous.Constructors().Returns(mixed);

                // Act
                IEnumerable<Constructor> actual = sut.Constructors();

                Assert.Equal(expected, actual);
            }
        }

        public class Events: InheritanceScopeTest
        {
            [Fact]
            public void ReturnsEventsInheritedFromAncestor() {
                // Arrange
                var expected = new Event[] {
                    new Event(EventInfo(MethodAttributes.Static).WithDeclaringType(parent)),
                    new Event(EventInfo(MethodAttributes.Static).WithDeclaringType(grandParent))
                };

                var mixed = new Event[] {
                    new Event(EventInfo(MethodAttributes.Static).WithDeclaringType(child)),
                    expected[0],
                    new Event(EventInfo(MethodAttributes.Static).WithDeclaringType(child)),
                    expected[1],
                    new Event(EventInfo(MethodAttributes.Static).WithDeclaringType(child)),
                };

                previous.Events().Returns(mixed);

                // Act
                IEnumerable<Event> actual = sut.Events();

                Assert.Equal(expected, actual);
            }
        }

        public class Fields: InheritanceScopeTest
        {
            [Fact]
            public void ReturnsFieldsInheritedFromAncestor() {
                // Arrange
                var expected = new Field[] {
                    new Field(FieldInfo(FieldAttributes.Static).WithDeclaringType(parent)),
                    new Field(FieldInfo(FieldAttributes.Static).WithDeclaringType(grandParent))
                };

                var mixed = new Field[] {
                    new Field(FieldInfo(FieldAttributes.Static).WithDeclaringType(child)),
                    expected[0],
                    new Field(FieldInfo(FieldAttributes.Static).WithDeclaringType(child)),
                    expected[1],
                    new Field(FieldInfo(FieldAttributes.Static).WithDeclaringType(child)),
                };

                previous.Fields().Returns(mixed);

                // Act
                IEnumerable<Field> actual = sut.Fields();

                Assert.Equal(expected, actual);
            }
        }

        public class Methods: InheritanceScopeTest
        {
            [Fact]
            public void ReturnsMethodsInheritedFromAncestor() {
                // Arrange
                var expected = new Method[] {
                    new Method(MethodInfo(MethodAttributes.Static).WithDeclaringType(parent)),
                    new Method(MethodInfo(MethodAttributes.Static).WithDeclaringType(grandParent))
                };

                var mixed = new Method[] {
                    new Method(MethodInfo(MethodAttributes.Static).WithDeclaringType(child)),
                    expected[0],
                    new Method(MethodInfo(MethodAttributes.Static).WithDeclaringType(child)),
                    expected[1],
                    new Method(MethodInfo(MethodAttributes.Static).WithDeclaringType(child)),
                };

                previous.Methods().Returns(mixed);

                // Act
                IEnumerable<Method> actual = sut.Methods();

                Assert.Equal(expected, actual);
            }
        }

        public class Properties: InheritanceScopeTest
        {
            [Fact]
            public void ReturnsPropertiesInheritedFromAncestor() {
                // Arrange
                var expected = new Property[] {
                    new Property(PropertyInfo(MethodAttributes.Static).WithDeclaringType(parent)),
                    new Property(PropertyInfo(MethodAttributes.Static).WithDeclaringType(grandParent))
                };

                var mixed = new Property[] {
                    new Property(PropertyInfo(MethodAttributes.Static).WithDeclaringType(child)),
                    expected[0],
                    new Property(PropertyInfo(MethodAttributes.Static).WithDeclaringType(child)),
                    expected[1],
                    new Property(PropertyInfo(MethodAttributes.Static).WithDeclaringType(child)),
                };

                previous.Properties().Returns(mixed);

                // Act
                IEnumerable<Property> actual = sut.Properties();

                Assert.Equal(expected, actual);
            }
        }

        class GrandParent { }
        class Parent: GrandParent { }
        class Child: Parent { }
    }
}
