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
        readonly IDelegateFactory<MethodInfo> delegateFactory = Substitute.For<IDelegateFactory<MethodInfo>>();

        // Test fixture
        readonly InstanceType instance = Substitute.ForPartsOf<InstanceType>();
        readonly Signature @delegate = Substitute.For<Signature>();

        public GenericMethodTest() {
            MethodInfo info = typeof(InstanceType).GetMethod(nameof(InstanceType.TestMethod));
            method = new Method(info, instance);

            delegateFactory.TryCreate(Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<MethodInfo>(), out Delegate _)
                .Returns(args => {
                    args[3] = @delegate;
                    return true;
                });

            sut = new Method<Signature>(method, delegateFactory);
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
                var thrown = Assert.Throws<ArgumentNullException>(() => new Method<Signature>(null, delegateFactory));
                Assert.Equal("method", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateFactoryIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Method<Signature>(method, null));
                Assert.Equal("delegateFactory", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateFactoryCannotCreateDelegateForGivenMethod() {
                delegateFactory.TryCreate(Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<MethodInfo>(), out Delegate _).Returns(false);

                var thrown = Assert.Throws<ArgumentException>(() => new Method<Signature>(method, delegateFactory));
                Assert.Equal("method", thrown.ParamName);
                Assert.StartsWith($"Method {method.Info} doesn't match expected signature.", thrown.Message);
            }
        }

        public class Invoke: GenericMethodTest
        {
            [Fact]
            public void ReturnsDelegateCreatedByDelegateFactoryForGivenMethod() {
                Assert.Same(@delegate, sut.Invoke);
                delegateFactory.Received().TryCreate(typeof(Signature), sut.Instance, sut.Info, out Delegate _);
                delegateFactory.Received(1).TryCreate(Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<MethodInfo>(), out Delegate _);
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
