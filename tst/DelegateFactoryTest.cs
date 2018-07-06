using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Xunit;

namespace Inspector
{
    public class DelegateFactoryTest
    {
        public class TryCreateFromConstructorInfo : DelegateFactoryTest
        {
            class T1
            {
                T1(P1 p) => P = p;
                public P1 P { get; }
            }

            class T2
            {
                T2(P1 p) { }
            }

            readonly T1 target = (T1)FormatterServices.GetUninitializedObject(typeof(T1));
            readonly P1 parameter = new P1();

            [Fact]
            public void CreatesOpenDelegateForConstructorWithMatchingParameters() {
                Assert.True(DelegateFactory.TryCreate(typeof(Action<T1, P1>), null, GetConstructor<T1>(), out Delegate @delegate));
                ((Action<T1, P1>)@delegate).Invoke(target, parameter);
                Assert.Same(parameter, target.P);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateForConstructorWithMismatchedParameters() {
                Assert.False(DelegateFactory.TryCreate(typeof(Action<T1, P2>), null, GetConstructor<T1>(), out Delegate @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateForConstructorOfDifferentType() {
                Assert.False(DelegateFactory.TryCreate(typeof(Action<T1, P2>), null, GetConstructor<T2>(), out Delegate @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateWithGivenTarget() {
                Assert.False(DelegateFactory.TryCreate(typeof(Action<T1, P1>), target, GetConstructor<T1>(), out Delegate @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void CreatesClosedDelegateForConstructorWithMatchingParameters() {
                Assert.True(DelegateFactory.TryCreate(typeof(Action<P1>), target, GetConstructor<T1>(), out Delegate @delegate));
                ((Action<P1>)@delegate).Invoke(parameter);
                Assert.Same(parameter, target.P);
            }

            [Fact]
            public void DoesNotCreateClosedDelegateForConstructorWithMismatchedParameters() {
                Assert.False(DelegateFactory.TryCreate(typeof(Action<P2>), target, GetConstructor<T1>(), out Delegate @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => DelegateFactory.TryCreate(null, target, GetConstructor<T1>(), out Delegate @delegate));
                Assert.Equal("type", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenConstructorInfoIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => DelegateFactory.TryCreate(typeof(Action<P1>), target, (ConstructorInfo)null, out Delegate @delegate));
                Assert.Equal("constructor", thrown.ParamName);
            }

            ConstructorInfo GetConstructor<T>() =>
                typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single();
        }

        public class TryCreateFromMethodInfo : DelegateFactoryTest
        {
            class T1
            {
                void M1(P1 p) => P = p;
                public P1 P { get; private set; }
            }

            class T2
            {
                void M1(P1 p) { }
            }

            readonly T1 target = new T1();
            readonly P1 parameter = new P1();

            [Fact]
            public void CreatesOpenDelegateForMethodWithMatchingParameters() {
                Assert.True(DelegateFactory.TryCreate(typeof(Action<T1, P1>), null, GetMethod<T1>(), out Delegate @delegate));
                ((Action<T1, P1>)@delegate).Invoke(target, parameter);
                Assert.Same(parameter, target.P);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateForMethodWithMismatchedParameters() {
                Assert.False(DelegateFactory.TryCreate(typeof(Action<T1, P2>), null, GetMethod<T1>(), out Delegate @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateForMethodOfDifferentType() {
                Assert.False(DelegateFactory.TryCreate(typeof(Action<T1, P2>), null, GetMethod<T2>(), out Delegate @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void DoesNotCreateOpenDelegateWithGivenTarget() {
                Assert.False(DelegateFactory.TryCreate(typeof(Action<T1, P1>), target, GetMethod<T1>(), out Delegate @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void CreatesClosedDelegateForMethodWithMatchingParameters() {
                Assert.True(DelegateFactory.TryCreate(typeof(Action<P1>), target, GetMethod<T1>(), out Delegate @delegate));
                ((Action<P1>)@delegate).Invoke(parameter);
                Assert.Same(parameter, target.P);
            }

            [Fact]
            public void DoesNotCreateClosedDelegateForMethodWithMismatchedParameters() {
                Assert.False(DelegateFactory.TryCreate(typeof(Action<P2>), target, GetMethod<T1>(), out Delegate @delegate));
                Assert.Null(@delegate);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => DelegateFactory.TryCreate(null, target, GetMethod<T1>(), out Delegate @delegate));
                Assert.Equal("type", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMethodInfoIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => DelegateFactory.TryCreate(typeof(Action<P1>), target, (MethodInfo)null, out Delegate @delegate));
                Assert.Equal("method", thrown.ParamName);
            }

            MethodInfo GetMethod<T>() =>
                typeof(T).GetMethod("M1", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        class P1 { }

        class P2 { }
    }
}
