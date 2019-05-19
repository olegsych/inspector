using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Xunit;

namespace Inspector
{
    public class ConstructorDelegateFactoryTest
    {
        readonly IDelegateFactory<ConstructorInfo> sut = new ConstructorDelegateFactory();

        public class TryCreate: ConstructorDelegateFactoryTest
        {
            // Method parameters
            readonly Type delegateType = typeof(Action<T1, P1>);
            readonly T1 target = (T1)FormatterServices.GetUninitializedObject(typeof(T1));
            readonly ConstructorInfo constructor = GetConstructor<T1>();
            Delegate @delegate = null;

            // Fixture
            readonly P1 parameter = new P1();

            [Fact]
            public void CreatesOpenDelegateForConstructorWithMatchingParameters() {
                Assert.True(sut.TryCreate(delegateType, null, constructor, out @delegate));
                ((Action<T1, P1>)@delegate).Invoke(target, parameter);
                Assert.Same(parameter, target.P);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateForConstructorWithMismatchedParameters() {
                Assert.False(sut.TryCreate(typeof(Action<T1, P2>), null, constructor, out @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateForConstructorOfDifferentType() {
                Assert.False(sut.TryCreate(delegateType, null, GetConstructor<T2>(), out @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateWithGivenTarget() {
                Assert.False(sut.TryCreate(delegateType, target, constructor, out @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void CreatesClosedDelegateForConstructorWithMatchingParameters() {
                Assert.True(sut.TryCreate(typeof(Action<P1>), target, constructor, out @delegate));
                ((Action<P1>)@delegate).Invoke(parameter);
                Assert.Same(parameter, target.P);
            }

            [Fact]
            public void DoesNotCreateClosedDelegateForConstructorWithMismatchedParameters() {
                Assert.False(sut.TryCreate(typeof(Action<P2>), target, constructor, out @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => sut.TryCreate(null, target, constructor, out @delegate));
                Assert.Equal("delegateType", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenConstructorInfoIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => sut.TryCreate(delegateType, target, null, out @delegate));
                Assert.Equal("method", thrown.ParamName);
            }

            class P1 { }

            class P2 { }

            class T1
            {
                T1(P1 p) => P = p;
                public P1 P { get; }
            }

            class T2
            {
                T2(P1 p) { }
            }

            static ConstructorInfo GetConstructor<T>() =>
                typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single();
        }
    }
}
