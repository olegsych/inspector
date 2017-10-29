using Inspector;
using NSubstitute;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace System
{
    [Collection(nameof(TypeInspector))]
    public class ObjectExtensionsTest : IDisposable
    {
        const BindingFlags allInstanceMembers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        readonly TypeInspector.Factory originalTypeInspectorCreate;
        readonly TypeInspector.Factory typeInspectorCreate = Substitute.For<TypeInspector.Factory>();
        readonly TypeInspector typeInspector = Substitute.For<TypeInspector>();

        public ObjectExtensionsTest()
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

        public class Constructor : ObjectExtensionsTest
        {
            readonly Base instance = new Derived();

            [Fact]
            public void CreatesTypeInspectorForGivenInstanceDefaultingToItsDeclaredType()
            {
                instance.Constructor();

                typeInspectorCreate.Received().Invoke(instance, typeof(Base));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<object>(), Arg.Any<Type>());
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

                typeInspectorCreate.Received().Invoke(instance, typeof(Base));
                typeInspectorCreate.Received(1).Invoke(Arg.Any<object>(), Arg.Any<Type>());
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
