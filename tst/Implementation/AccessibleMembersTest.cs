using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class AccessibleMembersTest
    {
        // Constructor parameters
        readonly IMembers source = Substitute.For<IMembers>();
        readonly Accessibility accessibility = Accessibility.PrivateProtected;

        public class Ctor: AccessibleMembersTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenSourceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new AccessibleMembers(null!, accessibility));
                Assert.Equal("source", thrown.ParamName);
            }

            [Fact]
            public void InitializesIDecoratorSourcePropertyForSelectorAccessToEntireFilterChain() {
                IDecorator<IMembers> sut = new AccessibleMembers(source, accessibility);
                Assert.Same(source, sut.Source);
            }

            [Fact]
            public void InitializesAccessiblityPropertyForUseInTests() {
                var sut = new AccessibleMembers(source, accessibility);
                Assert.Equal(accessibility, sut.Accessibility);
            }

            [Theory]
            [InlineData(Accessibility.Private, Accessibility.Protected, Accessibility.PrivateProtected)]
            [InlineData(Accessibility.Protected, Accessibility.Internal, Accessibility.ProtectedInternal)]
            internal void CombinesPrivateAndProtectedAccessibility(Accessibility first, Accessibility second, Accessibility combined) {
                var sut = new AccessibleMembers(new AccessibleMembers(source, first), second);

                Assert.Equal(combined, sut.Accessibility);
                Assert.Same(source, sut.Source);
            }

            [Theory, MemberData(nameof(InvalidAccessibilityCombinations))]
            internal void ThrowsDescriptiveExceptionWhenAccessibilitiesCannotBeCombined(Accessibility first, Accessibility second) {
                var thrown = Assert.Throws<InvalidOperationException>(() => new AccessibleMembers(new AccessibleMembers(source, first), second));
                Assert.StartsWith($"'{first.ToString().ToLower()} {second.ToString().ToLower()}' is not a valid accessibility.", thrown.Message);
            }

            public static IEnumerable<object[]> InvalidAccessibilityCombinations() {
                Accessibility[] accessibilities = { Accessibility.Internal, Accessibility.Private, Accessibility.PrivateProtected, Accessibility.Protected, Accessibility.ProtectedInternal, Accessibility.Public };
                foreach(Accessibility first in accessibilities)
                    foreach(Accessibility second in accessibilities)
                        if(!IsValidCombination(first, second))
                            yield return new object[] { first, second };
            }

            static bool IsValidCombination(Accessibility first, Accessibility second) =>
                first == Accessibility.Private && second == Accessibility.Protected ||
                first == Accessibility.Protected && second == Accessibility.Internal;
        }

        public class Constructors: AccessibleMembersTest
        {
            [Fact]
            public void ReturnsConstructorsWithWithExpectedAccessibility() {
                // Arrange
                var sut = new AccessibleMembers(source, Accessibility.ProtectedInternal);

                Constructor[] expected = {
                    new Constructor(ConstructorInfo(MethodAttributes.FamORAssem | MethodAttributes.Static)),
                    new Constructor(ConstructorInfo(MethodAttributes.FamORAssem | MethodAttributes.Static)),
                };

                Constructor[] all = {
                    new Constructor(ConstructorInfo(MethodAttributes.Public | MethodAttributes.Static)),
                    expected[0],
                    new Constructor(ConstructorInfo(MethodAttributes.Family | MethodAttributes.Static)),
                    expected[1],
                    new Constructor(ConstructorInfo(MethodAttributes.Private | MethodAttributes.Static)),
                };

                ConfiguredCall arrange = source.Constructors().Returns(all);

                // Act
                IEnumerable<Constructor> actual = sut.Constructors();

                // Assert
                Assert.Equal(expected, actual);
            }
        }

        public class Events: AccessibleMembersTest
        {
            [Fact]
            public void ReturnsEventsWithWithExpectedAccessibilityOfAddMethod() {
                // Arrange
                var sut = new AccessibleMembers(source, Accessibility.ProtectedInternal);

                Event[] expected = {
                    new Event(EventInfo(MethodAttributes.FamORAssem | MethodAttributes.Static)),
                    new Event(EventInfo(MethodAttributes.FamORAssem | MethodAttributes.Static)),
                };

                Event[] all = {
                    new Event(EventInfo(MethodAttributes.Public | MethodAttributes.Static)),
                    expected[0],
                    new Event(EventInfo(MethodAttributes.Family | MethodAttributes.Static)),
                    expected[1],
                    new Event(EventInfo(MethodAttributes.Private | MethodAttributes.Static)),
                };

                ConfiguredCall arrange = source.Events().Returns(all);

                // Act
                IEnumerable<Event> actual = sut.Events();

                // Assert
                Assert.Equal(expected, actual);
            }
        }

        public class Fields: AccessibleMembersTest
        {
            [Fact]
            public void ReturnsFieldsWithWithExpectedAccessibility() {
                // Arrange
                var sut = new AccessibleMembers(source, Accessibility.ProtectedInternal);

                Field[] expected = {
                    new Field(FieldInfo(FieldAttributes.FamORAssem | FieldAttributes.Static)),
                    new Field(FieldInfo(FieldAttributes.FamORAssem | FieldAttributes.Static)),
                };

                Field[] all = {
                    new Field(FieldInfo(FieldAttributes.Public | FieldAttributes.Static)),
                    expected[0],
                    new Field(FieldInfo(FieldAttributes.Family | FieldAttributes.Static)),
                    expected[1],
                    new Field(FieldInfo(FieldAttributes.Private | FieldAttributes.Static)),
                };

                ConfiguredCall arrange = source.Fields().Returns(all);

                // Act
                IEnumerable<Field> actual = sut.Fields();

                // Assert
                Assert.Equal(expected, actual);
            }
        }

        public class Methods: AccessibleMembersTest
        {
            [Fact]
            public void ReturnsMethodsWithWithExpectedAccessibility() {
                // Arrange
                var sut = new AccessibleMembers(source, Accessibility.ProtectedInternal);

                Method[] expected = {
                    new Method(MethodInfo(MethodAttributes.FamORAssem | MethodAttributes.Static)),
                    new Method(MethodInfo(MethodAttributes.FamORAssem | MethodAttributes.Static)),
                };

                Method[] all = {
                    new Method(MethodInfo(MethodAttributes.Public | MethodAttributes.Static)),
                    expected[0],
                    new Method(MethodInfo(MethodAttributes.Family | MethodAttributes.Static)),
                    expected[1],
                    new Method(MethodInfo(MethodAttributes.Private | MethodAttributes.Static)),
                };

                ConfiguredCall arrange = source.Methods().Returns(all);

                // Act
                IEnumerable<Method> actual = sut.Methods();

                // Assert
                Assert.Equal(expected, actual);
            }
        }

        public class Properties: AccessibleMembersTest
        {
            [Fact]
            public void ReturnsPropertiesWithWithExpectedAccessibility() {
                // Arrange
                var sut = new AccessibleMembers(source, Accessibility.ProtectedInternal);

                Property[] expected = {
                    new Property(PropertyInfo(MethodAttributes.FamORAssem | MethodAttributes.Static)),
                    new Property(PropertyInfo(MethodAttributes.FamORAssem | MethodAttributes.Static)),
                };

                Property[] all = {
                    new Property(PropertyInfo(MethodAttributes.Public | MethodAttributes.Static)),
                    expected[0],
                    new Property(PropertyInfo(MethodAttributes.Family | MethodAttributes.Static)),
                    expected[1],
                    new Property(PropertyInfo(MethodAttributes.Private | MethodAttributes.Static)),
                };

                ConfiguredCall arrange = source.Properties().Returns(all);

                // Act
                IEnumerable<Property> actual = sut.Properties();

                // Assert
                Assert.Equal(expected, actual);
            }
        }
    }
}
