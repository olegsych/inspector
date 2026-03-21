using System;
using System.Reflection;
using Xunit;

namespace Inspector.Implementation
{
    public class MethodDelegateFactoryTest
    {
        readonly IDelegateFactory<MethodInfo> sut = new MethodDelegateFactory();

        public class TryCreate: MethodDelegateFactoryTest
        {
            // Method parameters
            readonly Type delegateType = typeof(Action<T1, P1>);
            readonly T1 target = new T1();
            readonly MethodInfo method = GetMethod<T1>();
            Delegate? @delegate = null;

            // Fixture
            readonly P1 parameter = new P1();

            [Fact]
            public void CreatesOpenDelegateForMethodWithMatchingParameters() {
                Assert.True(sut.TryCreate(delegateType, null, method, out @delegate));
                ((Action<T1, P1>)@delegate!).Invoke(target, parameter);
                Assert.Same(parameter, target.P);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateForMethodWithMismatchedParameters() {
                Assert.False(sut.TryCreate(typeof(Action<T1, P2>), null, method, out @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateForMethodOfDifferentType() {
                Assert.False(sut.TryCreate(delegateType, null, GetMethod<T2>(), out @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateWithGivenTarget() {
                Assert.False(sut.TryCreate(delegateType, target, method, out @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void CreatesClosedDelegateForMethodWithMatchingParameters() {
                Assert.True(sut.TryCreate(typeof(Action<P1>), target, method, out @delegate));
                ((Action<P1>)@delegate!).Invoke(parameter);
                Assert.Same(parameter, target.P);
            }

            [Fact]
            public void DoesNotCreateClosedDelegateForMethodWithMismatchedParameters() {
                Assert.False(sut.TryCreate(typeof(Action<P2>), target, method, out @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => sut.TryCreate(null!, target, method, out @delegate));
                Assert.Equal("delegateType", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodInfoIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => sut.TryCreate(delegateType, target, null!, out @delegate));
                Assert.Equal("method", thrown.ParamName);
            }

            class P1 { }

            class P2 { }

            class T1
            {
                void M(P1 p) => P = p;
                public P1? P { get; private set; }
            }

            class T2
            {
                void M(P1 p) { }
            }

            static MethodInfo GetMethod<T>() =>
                typeof(T).GetMethod("M", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new ArgumentException();
        }
    }
}
