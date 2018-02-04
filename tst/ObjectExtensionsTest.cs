using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest : TypeInspectorFixture
    {
        public class Constructor : ObjectExtensionsTest
        {
            readonly Base instance = new Derived();

            [Fact]
            public void CreatesTypeInspectorForGivenInstanceDefaultingToItsDeclaredType()
            {
                instance.Constructor();

                typeInspectorCreate.Received().Invoke(typeof(Base), instance);
                typeInspectorCreate.Received(1).Invoke(Arg.Any<Type>(), Arg.Any<object>());
            }

            [Fact]
            public void PassesGivenParametersToTypeInspector()
            {
                var parameters = new Type[2];

                instance.Constructor(parameters);

                typeInspector.Received().GetConstructor(parameters);
                typeInspector.Received(1).GetConstructor(Arg.Any<Type[]>());
            }

            [Fact]
            public void ReturnsConstructorReceivedFromTypeInspector()
            {
                var expected = Substitute.For<ConstructorInfo>();
                typeInspector.GetConstructor().Returns(expected);

                ConstructorInfo actual = instance.Constructor();

                Assert.Same(expected, actual);
            }
        }

        public class Constructors : ObjectExtensionsTest
        {
            readonly Base instance = new Derived();

            [Fact]
            public void CreatesTypeInspectorForGivenIstanceDefaultingToItsDeclaredType()
            {
                instance.Constructors();

                typeInspectorCreate.Received().Invoke(typeof(Base), instance);
                typeInspectorCreate.Received(1).Invoke(Arg.Any<Type>(), Arg.Any<object>());
            }

            [Fact]
            public void ReturnsConstructorsReceivedFromTypeInspector()
            {
                var expected = new ConstructorInfo[0];
                typeInspector.GetConstructors().Returns(expected);

                IReadOnlyList<ConstructorInfo> actual = instance.Constructors();

                Assert.Same(expected, actual);
            }
        }

        class Base { }

        class Derived : Base { }
    }
}
