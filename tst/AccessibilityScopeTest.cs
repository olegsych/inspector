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
        readonly IEnumerable<AccessModifier> accessModifiers = Substitute.For<IEnumerable<AccessModifier>>();

        public class Constructor : AccessibilityScopeTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousScopeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new AccessibilityScope(null, accessModifiers));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptoinWhenAccessModifiersIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new AccessibilityScope(previous, null));
                Assert.Equal("accessModifiers", thrown.ParamName);
            }
        }

        public class Previous : AccessibilityScopeTest
        {
            [Fact]
            public void ImplementsIDecoratorToAllowSelectorAccessToEntireFilterChain() {
                IDecorator<IScope> sut = new AccessibilityScope(previous, accessModifiers);
                Assert.Same(previous, sut.Previous);
            }
        }

        public class AccessModifiers : AccessibilityScopeTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructorForUseInTests() {
                var sut = new AccessibilityScope(previous, accessModifiers);
                Assert.Same(accessModifiers, sut.AccessModifiers);
            }
        }

        public class GetFields : AccessibilityScopeTest
        {
            [Fact]
            public void ReturnsFieldsWithWithExpectedAccessModifiers() {
                // Arrange
                var sut = new AccessibilityScope(previous, new[] { AccessModifier.Internal, AccessModifier.ProtectedInternal });

                Field[] expected = {
                    new Field(FieldInfo(FieldAttributes.Assembly | FieldAttributes.Static)),
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

                Assert.Equal(expected, actual);
            }
        }
    }
}
