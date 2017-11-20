using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector.System
{
    [Collection(nameof(TypeInspector))]
    public class TypeExtensionsTest : IDisposable
    {
        readonly TypeInspector.Factory originalTypeInspectorCreate;
        readonly TypeInspector.Factory typeInspectorCreate = Substitute.For<TypeInspector.Factory>();
        readonly TypeInspector typeInspector = Substitute.For<TypeInspector>();

        public TypeExtensionsTest()
        {
            typeInspectorCreate.Invoke(Arg.Any<object>(), Arg.Any<Type>()).Returns(typeInspector);
            originalTypeInspectorCreate = ReplaceTypeInspectorCreate(typeInspectorCreate);
        }

        void IDisposable.Dispose()
        {
            ReplaceTypeInspectorCreate(originalTypeInspectorCreate);
        }

        static TypeInspector.Factory ReplaceTypeInspectorCreate(TypeInspector.Factory replacement)
        {
            FieldInfo create = typeof(TypeInspector).GetField(nameof(TypeInspector.Create));
            var current = (TypeInspector.Factory)create.GetValue(null);
            create.SetValue(null, replacement);
            return current;
        }

        public class Constructor : TypeExtensionsTest
        {
            [Fact]
            public void CreatesTypeInspectorForGivenType()
            {
                typeof(TestClass).Constructor();

                typeInspectorCreate.Received().Invoke(null, typeof(TestClass));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<object>(), Arg.Any<Type>());
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

                typeInspectorCreate.Received().Invoke(null, typeof(TestClass));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<object>(), Arg.Any<Type>());
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
