using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector
{
    public class TypeInspectorTest
    {
        public class Create: TypeInspectorTest
        {
            [Fact]
            public void CreatesInspectorOfGivenInstanceType() {
                var inspector = TypeInspector.Create(typeof(object), new TestClass());
                AssertInspectorType(typeof(TestClass), inspector);
            }

            [Fact]
            public void CreatesInspectorOfDeclaredTypeWhenInstanceIsNull() {
                var inspector = TypeInspector.Create(typeof(TestClass), null);
                AssertInspectorType(typeof(TestClass), inspector);
            }

            static void AssertInspectorType(Type expected, TypeInspector inspector) {
                var actualType = (TypeInfo)inspector.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(_ => _.FieldType == typeof(TypeInfo)).GetValue(inspector);
                Assert.Equal(expected.GetTypeInfo(), actualType);
            }

            class TestClass { }
        }

        public class GetConstructor: TypeInspectorTest
        {
            [Fact]
            public void ReturnsSingleParameterlessConstructorOfGivenType() {
                var sut = Substitute.ForPartsOf<TypeInspector>(typeof(TestClass).GetTypeInfo());
                var expected = Substitute.For<ConstructorInfo>();
                sut.GetConstructors().Returns(new[] { expected });

                ConstructorInfo actual = sut.GetConstructor();

                Assert.Same(expected, actual);
            }

            [Fact]
            public void ReturnsConstructorWithMatchingParameterTypes() {
                var sut = Substitute.ForPartsOf<TypeInspector>(typeof(TestClass).GetTypeInfo());
                ConstructorInfo expected = ConstructorInfo(typeof(Foo), typeof(Bar));
                sut.GetConstructors().Returns(new[] { expected });

                ConstructorInfo actual = sut.GetConstructor(typeof(Foo), typeof(Bar));

                Assert.Same(expected, actual);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenTypeDoesntHaveConstructorWithMatchingParameters() {
                var sut = Substitute.ForPartsOf<TypeInspector>(typeof(TestClass).GetTypeInfo());
                ConstructorInfo unexpected = ConstructorInfo(typeof(Bar), typeof(Foo));
                sut.GetConstructors().Returns(new[] { unexpected });

                var thrown = Assert.Throws<ArgumentException>(() => sut.GetConstructor(typeof(Foo), typeof(Bar)));

                Assert.Equal("parameterTypes", thrown.ParamName);
                Assert.StartsWith($"Type {nameof(TestClass)} doesn't have a constructor with parameter types({nameof(Foo)}, {nameof(Bar)})", thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenParameterTypesIsNullToFailFast() {
                var sut = Substitute.ForPartsOf<TypeInspector>(typeof(TestClass).GetTypeInfo());

                var thrown = Assert.Throws<ArgumentNullException>(() => sut.GetConstructor(default(Type[])));

                Assert.Equal("parameterTypes", thrown.ParamName);
            }

            class TestClass { }

            class Foo { }

            class Bar { }

            static ConstructorInfo ConstructorInfo(params Type[] parameterTypes) {
                var constructor = Substitute.For<ConstructorInfo>();
                ParameterInfo[] parameters = parameterTypes.Select(t => ParameterInfo(t)).ToArray();
                constructor.GetParameters().Returns(parameters);
                return constructor;
            }
        }

        public class GetConstructors: TypeInspectorTest
        {
            [Fact]
            public void ReturnsAllConstructorsOfGivenType() {
                var sut = Substitute.ForPartsOf<TypeInspector>(typeof(TestClass).GetTypeInfo());
                IEnumerable<ConstructorInfo> expected = typeof(TestClass).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                IEnumerable<ConstructorInfo> actual = sut.GetConstructors();

                Assert.Equal(expected, actual);
            }

            class TestClass
            {
                public TestClass() { }
                internal TestClass(string _) { }
                protected TestClass(char _) { }
                TestClass(bool _) { }
            }
        }
    }
}
