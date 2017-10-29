using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class TypeInspectorTest
    {
        public class Create : TypeInspectorTest
        {
            [Fact]
            public void CreatesInspectorOfGivenInstanceType()
            {
                var inspector = TypeInspector.Create(new TestClass(), typeof(object));
                AssertInspectorType(typeof(TestClass), inspector);
            }

            [Fact]
            public void CreatesInspectorOfDefaultTypeWhenInstanceIsNull()
            {
                var inspector = TypeInspector.Create(null, typeof(TestClass));
                AssertInspectorType(typeof(TestClass), inspector);
            }

            static void AssertInspectorType(Type expected, TypeInspector inspector)
            {
                var actualType = (TypeInfo)inspector.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(_ => _.FieldType == typeof(TypeInfo)).GetValue(inspector);
                Assert.Equal(expected.GetTypeInfo(), actualType);
            }

            class TestClass { }
        }

        public class GetConstructor : TypeInspectorTest
        {
            [Fact]
            public void ReturnsSingleConstructorOfGivenType()
            {
                var sut = Substitute.ForPartsOf<TypeInspector>(typeof(TestClass).GetTypeInfo());
                var expected = Substitute.For<ConstructorInfo>();
                sut.GetConstructors().Returns(new[] { expected });

                ConstructorInfo actual = sut.GetConstructor();

                Assert.Same(expected, actual);
            }

            class TestClass { }
        }

        public class GetConstructors : TypeInspectorTest
        {
            [Fact]
            public void ReturnsAllConstructorsOfGivenType()
            {
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
