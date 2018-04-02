using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class AccessibilityScopeTest
    {
        // Constructor parameters
        readonly IScope previous = Substitute.For<IScope>();
        readonly Accessibility accessibility = Accessibility.PrivateProtected;

        public class Constructor : AccessibilityScopeTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousScopeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new AccessibilityScope(null, accessibility));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void InitializesIDecoratorPreviousPropertyForSelectorAccessToEntireFilterChain() {
                IDecorator<IScope> sut = new AccessibilityScope(previous, accessibility);
                Assert.Same(previous, sut.Previous);
            }

            [Fact]
            public void InitializesAccessiblityPropertyForUseInTests() {
                var sut = new AccessibilityScope(previous, accessibility);
                Assert.Equal(accessibility, sut.Accessibility);
            }
        }

        public class GetFields : AccessibilityScopeTest
        {
            [Fact]
            public void ReturnsFieldsWithWithExpectedAccessibility() {
                // Arrange
                var sut = new AccessibilityScope(previous, Accessibility.ProtectedInternal);

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

                ((IFilter<Field>)previous).Get().Returns(all);

                // Act
                IEnumerable<Field> actual = ((IFilter<Field>)sut).Get();

                // Assert
                Assert.Equal(expected, actual);
            }
        }
    }
}
