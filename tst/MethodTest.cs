using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class MethodTest
    {
        readonly Method sut;

        // Constructor parameters
        readonly MethodInfo instanceMethod = typeof(TestType).GetMethod(nameof(TestType.Method));
        readonly MethodInfo staticMethod = typeof(TestType).GetMethod(nameof(TestType.StaticMethod));
        readonly object instance = Substitute.For<TestType>();

        public MethodTest() =>
            sut = new Method(instanceMethod, instance);

        public class Constructor : MethodTest
        {
            [Fact]
            public void InitializesNewInstanceForInstanceMethod() {
                Member<MethodInfo> member = sut;

                Assert.Same(instanceMethod, member.Info);
                Assert.Same(instance, member.Instance);
            }

            [Fact]
            public void InitializesNewInstanceForStaticMethod() {
                Member<MethodInfo> member = new Method(staticMethod);

                Assert.Same(staticMethod, member.Info);
                Assert.Null(member.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenGivenNoInstanceForInstanceMethod() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Method(instanceMethod));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance is required for method {instanceMethod.Name}", thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenGivenInstanceForStaticMEthod() {
                var thrown = Assert.Throws<ArgumentException>(() => new Method(staticMethod, instance));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance shouldn't be specified for static method {staticMethod.Name}.", thrown.Message);
            }
        }

        public class Create : MethodTest
        {
            [Fact]
            public void ReturnsMethodWithGivenMethodInfoAndInstance() {
                Method actual = Method.Create(instanceMethod, instance);

                Assert.Same(instanceMethod, actual.Info);
                Assert.Same(instance, actual.Instance);
            }
        }

        public class Invoke : MethodTest
        {
            [Fact]
            public void CallsMethodAndReturnsItsResult() {
                var expectedParameter = new ParameterType();
                var expectedResult = new ReturnType();
                ((TestType)instance).Method(expectedParameter).Returns(expectedResult);

                object actualResult = sut.Invoke(expectedParameter);

                Assert.Same(expectedResult, actualResult);
            }
        }

        public class TestType
        {
            public virtual ReturnType Method(ParameterType p) => null;
            public static ReturnType StaticMethod(ParameterType p) => null;
        }

        public class ParameterType { }

        public class ReturnType { }
    }
}
