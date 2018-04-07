using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class GenericMethodTest
    {
        readonly Method<Signature> sut;

        // Constructor parameters
        readonly Method method;

        // Test fixture
        readonly InstanceType instance = Substitute.ForPartsOf<InstanceType>();

        public GenericMethodTest() {
            MethodInfo info = typeof(InstanceType).GetMethod(nameof(InstanceType.TestMethod));
            method = new Method(info, instance);

            sut = new Method<Signature>(method);
        }

        public class Constructor : GenericMethodTest
        {
            [Fact]
            public void InitializesBaseWithGivenArgument() {
                Method @base = sut;
                Assert.Same(method.Info, @base.Info);
                Assert.Same(method.Instance, @base.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Method<Signature>(null));
                Assert.Equal("method", thrown.ParamName);
            }

            [Theory]
            [InlineData(nameof(InstanceType.MethodWithFewerParameters))]
            [InlineData(nameof(InstanceType.MethodWithDifferentParameterTypes))]
            [InlineData(nameof(InstanceType.MethodWithMoreParameters))]
            [InlineData(nameof(InstanceType.MethodWithDifferentReturnType))]
            public void ThrowsDescriptiveExceptionWhenInfoDoesNotHaveExpectedSignature(string methodName) {
                MethodInfo unexpected = typeof(InstanceType).GetMethod(methodName);
                var thrown = Assert.Throws<ArgumentException>(() => new Method<Signature>(new Method(unexpected, new InstanceType())));
                Assert.Equal("method", thrown.ParamName);
                Assert.StartsWith($"Method {unexpected} doesn't match expected signature.", thrown.Message);
            }
        }

        public class Invoke: GenericMethodTest
        {
            [Fact]
            public void CallsMethodAndReturnsItsResult() {
                var r1 = new R1();
                var p1 = new P1();
                var p2 = new P2();
                instance.TestMethod(p1, p2).Returns(r1);

                R1 actual = sut.Invoke(p1, p2);

                Assert.Same(r1, actual);
            }
        }

        public class ImplicitOperatorT : GenericMethodTest
        {
            [Fact]
            public void ImplicitlyConvertsMethodToItsSignatureDelegate() {
                Signature actual = sut;
                Assert.Same(sut.Invoke, actual);
            }

            [Fact]
            public void ConvertsNullMethodToNullSignatureToSupportImplicitConversionRules() {
                Signature actual = ((Method<Signature>)null);
                Assert.Null(actual);
            }
        }

        internal class InstanceType
        {
            public virtual R1 TestMethod(P1 p1, P2 p2) => null;
            public R1 MethodWithFewerParameters(P1 p1) => null;
            public R1 MethodWithDifferentParameterTypes(P2 p2, P1 p1) => null;
            public R1 MethodWithMoreParameters(P1 p1, P2 p2, P3 p3) => null;
            public R2 MethodWithDifferentReturnType(P1 p1, P2 p2) => null;
        }

        internal class P1 { }

        internal class P2 { }

        internal class P3 { }

        internal class R1 { }

        internal class R2 { }

        delegate R1 Signature(P1 p1, P2 p2);
    }
}
