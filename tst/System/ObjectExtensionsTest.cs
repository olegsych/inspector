using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System
{
    public class ObjectExtensionsTest
    {
        public class Constructor : ObjectExtensionsTest
        {
            [Theory, MemberData(nameof(TestInstances))]
            public void ReturnsConsturctorOfGivenInstance(object instance)
            {
                ConstructorInfo actual = instance.Constructor();
                ConstructorInfo expected = instance.GetType().GetConstructors().Single();
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
                ConstructorInfo actual = instance.Constructor();
                ConstructorInfo expected = typeof(T).GetConstructors().Single();
                Assert.Equal(expected, actual);
            }

            public static IEnumerable<object[]> TestInstances() 
            {
                yield return new object[] { new TestClass() };
                yield return new object[] { new TestClassWithSingleParameterizedConstructor(null) };
            }

            class TestClass { }

            class TestClassWithSingleParameterizedConstructor
            {
                public TestClassWithSingleParameterizedConstructor(TestClass _)
                {
                }
            }
        }
    }
}
