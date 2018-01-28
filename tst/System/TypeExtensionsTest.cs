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

        public class GenericConstructor : TypeExtensionsTest
        {
            class P1 { }
            class P2 { }
            class P3 { }
            class P4 { }
            class P5 { }
            class P6 { }
            class P7 { }
            class P8 { }

            public static IEnumerable<object[]> GenericParameters()
            {
                Type[] parameters = new[] { typeof(P1), typeof(P2), typeof(P3), typeof(P4), typeof(P5), typeof(P6), typeof(P7), typeof(P8) };
                for (byte numberOfParameters = 1; numberOfParameters <= 8; numberOfParameters++)
                    yield return new object[] { parameters.Take(numberOfParameters).ToArray() };
            }

            [Theory, MemberData(nameof(GenericParameters))]
            public void CreatesTypeInspectorForGivenType(Type[] parameters)
            {
                InvokeGenericConstructorMethod(parameters);

                typeInspectorCreate.Received().Invoke(typeof(TestClass));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<Type>());
            }

            [Theory, MemberData(nameof(GenericParameters))]
            public void PassesGivenParametersToTypeInspector(Type[] parameters)
            {
                InvokeGenericConstructorMethod(parameters);

                typeInspector.Received().GetConstructor(parameters);
                typeInspector.Received(1).GetConstructor(Arg.Any<Type[]>());
            }

            [Theory, MemberData(nameof(GenericParameters))]
            public void ReturnsConstructorObtainedFromTypeInspector(Type[] parameters)
            {
                var expected = Substitute.For<ConstructorInfo>();
                typeInspector.GetConstructor(Arg.Any<Type[]>()).Returns(expected);

                ConstructorInfo actual = InvokeGenericConstructorMethod(parameters);

                Assert.Same(expected, actual);
            }

            ConstructorInfo InvokeGenericConstructorMethod(Type[] parameters)
            {
                MethodInfo genericDefinition = typeof(global::System.TypeExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(_ => _.IsGenericMethod
                             && _.Name == nameof(global::System.TypeExtensions.Constructor)
                             && _.GetGenericArguments().Length == parameters.Length)
                    .SingleOrDefault();

                Assert.True(genericDefinition != null, $"No Constructor<> method with {parameters.Length} type parameters");

                MethodInfo genericMethod = genericDefinition.MakeGenericMethod(parameters);

                return (ConstructorInfo)genericMethod.Invoke(null, new object[] { typeof(TestClass) });
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
