using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector.System
{
    public class TypeExtensionsTest : TypeInspectorFixture
    {
        public class Constructor : TypeExtensionsTest
        {
            [Fact]
            public void CreatesTypeInspectorForGivenType()
            {
                typeof(TestClass).Constructor();

                typeInspectorCreate.Received().Invoke(typeof(TestClass));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<Type>());
            }

            [Fact]
            public void ReturnsConstructorObtainedFromTypeInspector()
            {
                var expected = Substitute.For<ConstructorInfo>();
                typeInspector.GetConstructor().Returns(expected);

                ConstructorInfo actual = typeof(TestClass).Constructor();

                Assert.Same(expected, actual);
            }
        }

        public class Constructors : TypeExtensionsTest
        {
            [Fact]
            public void CreatesTypeInspectorForGivenType()
            {
                typeof(TestClass).Constructors();

                typeInspectorCreate.Received().Invoke(typeof(TestClass));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<Type>());
            }

            [Fact]
            public void ReturnsConstructorsObtainedFromTypeInspector()
            {
                IReadOnlyList<ConstructorInfo> expected = new ConstructorInfo[0];
                typeInspector.GetConstructors().Returns(expected);

                IReadOnlyList<ConstructorInfo> actual = typeof(TestClass).Constructors();

                Assert.Same(expected, actual);
            }
        }

        class TestClass { }
    }
}
