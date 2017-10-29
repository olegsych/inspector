using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System
{
    public class ObjectExtensionsTest
    {
        const BindingFlags allInstanceMembers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public class Constructor : ObjectExtensionsTest
        {
            [Theory, MemberData(nameof(TestInstances))]
            public void ReturnsConsturctorOfGivenInstance(object instance)
            {
                ConstructorInfo expected = instance.GetType().GetConstructors(allInstanceMembers).Single();

                ConstructorInfo actual = instance.Constructor();

                Assert.Equal(expected, actual);
            }

            [Theory, MemberData(nameof(TestInstances))]
            public void ReturnsParameterlessConstructorOfGivenTypeWhenInstanceIsNull(object instance)
            {
                var assert = (Action)GetType()
                    .GetMethod(nameof(AssertConstructorReturnsParameterlessConstructorOfGivenType), BindingFlags.Static | BindingFlags.NonPublic)
                    .MakeGenericMethod(instance.GetType())
                    .CreateDelegate(typeof(Action));
                assert();
            }

            static void AssertConstructorReturnsParameterlessConstructorOfGivenType<T>()
            {
                T instance = default;
                ConstructorInfo expected = typeof(T).GetConstructors(allInstanceMembers).Single();

                ConstructorInfo actual = instance.Constructor();

                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> TestInstances() 
            {
                yield return new object[] { new ClassWithPublicConstructor() };
                yield return new object[] { new ClassWithParameterizedConstructor(null) };
                yield return new object[] { new ClassWithInternalConstructor() };
            }

            class ClassWithPublicConstructor { }

            class ClassWithParameterizedConstructor
            {
                public ClassWithParameterizedConstructor(ClassWithPublicConstructor _) { }
            }

            class ClassWithInternalConstructor
            {
                internal ClassWithInternalConstructor() { }
            }

            class ClassWithPrivateConstructor
            {
                ClassWithPrivateConstructor() { }
            }
        }

        public class Constructors : ObjectExtensionsTest
        {
            [Fact]
            public void ReturnsAllConstructorsOfGivenInstance()
            {
                object instance = new TestClass();
                IEnumerable<ConstructorInfo> expected = instance.GetType().GetConstructors(allInstanceMembers);

                IEnumerable<ConstructorInfo> actual = instance.Constructors();

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsAllConstructorsOfGivenTypeWhenInstanceIsNull()
            {
                TestClass instance = null;
                IEnumerable<ConstructorInfo> expected = typeof(TestClass).GetConstructors(allInstanceMembers);

                IEnumerable<ConstructorInfo> actual = instance.Constructors();

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
