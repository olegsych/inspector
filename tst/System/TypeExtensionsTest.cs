using System;
using System.Collections.Generic;
using System.Linq;
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
            public void PassesGivenParametersToTypeInspector()
            {
                var parameters = new Type[2];

                typeof(TestClass).Constructor(parameters);

                typeInspector.Received().GetConstructor(parameters);
                typeInspector.Received(1).GetConstructor(Arg.Any<Type[]>());
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

        class P1 { }
        class P2 { }
        class P3 { }

        public class ConstructorT : TypeExtensionsTest
        {
            [Fact]
            public void CreatesTypeInspectorForGivenType()
            {
                typeof(TestClass).Constructor<P1>();

                typeInspectorCreate.Received().Invoke(typeof(TestClass));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<Type>());
            }

            [Fact]
            public void PassesGivenParametersToTypeInspector()
            {
                typeof(TestClass).Constructor<P1>();

                typeInspector.Received().GetConstructor(typeof(P1));
                typeInspector.Received(1).GetConstructor(Arg.Any<Type[]>());
            }

            [Fact]
            public void ReturnsConstructorObtainedFromTypeInspector()
            {
                var expected = Substitute.For<ConstructorInfo>();
                typeInspector.GetConstructor(Arg.Any<Type[]>()).Returns(expected);

                ConstructorInfo actual = typeof(TestClass).Constructor<P1>();

                Assert.Same(expected, actual);
            }
        }

        public class ConstructorT1T2 : TypeExtensionsTest
        {
            [Fact]
            public void CreatesTypeInspectorForGivenType()
            {
                typeof(TestClass).Constructor<P1, P2>();

                typeInspectorCreate.Received().Invoke(typeof(TestClass));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<Type>());
            }

            [Fact]
            public void PassesGivenParametersToTypeInspector()
            {
                typeof(TestClass).Constructor<P1, P2>();

                typeInspector.Received().GetConstructor(typeof(P1), typeof(P2));
                typeInspector.Received(1).GetConstructor(Arg.Any<Type[]>());
            }

            [Fact]
            public void ReturnsConstructorObtainedFromTypeInspector()
            {
                var expected = Substitute.For<ConstructorInfo>();
                typeInspector.GetConstructor(Arg.Any<Type[]>()).Returns(expected);

                ConstructorInfo actual = typeof(TestClass).Constructor<P1, P2>();

                Assert.Same(expected, actual);
            }
        }

        public class ConstructorT1T2T3 : TypeExtensionsTest
        {
            [Fact]
            public void CreatesTypeInspectorForGivenType()
            {
                typeof(TestClass).Constructor<P1, P2, P3>();

                typeInspectorCreate.Received().Invoke(typeof(TestClass));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<Type>());
            }

            [Fact]
            public void PassesGivenParametersToTypeInspector()
            {
                typeof(TestClass).Constructor<P1, P2, P3>();

                typeInspector.Received().GetConstructor(typeof(P1), typeof(P2), typeof(P3));
                typeInspector.Received(1).GetConstructor(Arg.Any<Type[]>());
            }

            [Fact]
            public void ReturnsConstructorObtainedFromTypeInspector()
            {
                var expected = Substitute.For<ConstructorInfo>();
                typeInspector.GetConstructor(Arg.Any<Type[]>()).Returns(expected);

                ConstructorInfo actual = typeof(TestClass).Constructor<P1, P2, P3>();

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
