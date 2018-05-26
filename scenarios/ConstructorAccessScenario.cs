using System;
using System.Reflection;
using System.Runtime.Serialization;
using Xunit;

namespace Inspector
{
    public class ConstructorAccessScenario
    {
        public class Invoke : ConstructorAccessScenario
        {
            class Foo
            {
                public int bar;
                public Foo(int bar) => this.bar = bar;

                public static int staticBar;

                static Foo() {
                    staticBar = 0;
                }
            }

            [Fact]
            public void ConstructorCreatesNewInstance() {
                ConstructorInfo constructor = typeof(Foo).Constructor<int>();
                Assert.False(constructor.IsStatic);

                object foo = constructor.Invoke(new object[] { 42 });

                var typedFoo = Assert.IsType<Foo>(foo);
                Assert.Equal(42, typedFoo.bar);
            }

            [Fact]
            public void ConstructorReinitializesExistingInstance() {
                ConstructorInfo constructor = typeof(Foo).Constructor<int>();
                Assert.False(constructor.IsStatic);
                var foo = new Foo(0);

                object result = constructor.Invoke(foo, new object[] { 42 });

                Assert.Null(result);
                Assert.Equal(42, foo.bar);
            }

            [Fact]
            public void StaticConstructorReinitializesType() {
                ConstructorInfo constructor = typeof(Foo).TypeInitializer;
                Assert.True(constructor.IsStatic);
                Foo.staticBar = 42;

                constructor.Invoke(null, null);

                Assert.Equal(0, Foo.staticBar);
            }

            [Fact]
            public void ConstructorDelegate() {
                ConstructorInfo actionInfo = typeof(Action<Foo, int>).GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
                Assert.NotNull(actionInfo);

                ConstructorInfo fooInfo = typeof(Foo).GetConstructor(new Type[] { typeof(int) });
                Assert.NotNull(fooInfo);

                var ctor = (Action<Foo, int>)actionInfo.Invoke(new object[] { null, fooInfo.MethodHandle.GetFunctionPointer() });
                Assert.NotNull(ctor);

                var foo = (Foo)FormatterServices.GetUninitializedObject(typeof(Foo));
                ctor.Invoke(foo, 42);

                Assert.Equal(42, foo.bar);
            }

            [Fact]
            public void ConstructorDelegate2() {
                ConstructorInfo actionInfo = typeof(Action<int>).GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
                Assert.NotNull(actionInfo);

                ConstructorInfo fooInfo = typeof(Foo).GetConstructor(new Type[] { typeof(int) });
                Assert.NotNull(fooInfo);

                var foo = (Foo)FormatterServices.GetUninitializedObject(typeof(Foo));

                var ctor = (Action<int>)actionInfo.Invoke(new object[] { foo, fooInfo.MethodHandle.GetFunctionPointer() });
                Assert.NotNull(ctor);

                ctor.Invoke(42);

                Assert.Equal(42, foo.bar);
            }

            [Fact]
            public void Signature() {
                var signatureType = Type.GetType("System.Signature");
                Assert.NotNull(signatureType);

                var iRuntimeMethodInfoType = Type.GetType("System.IRuntimeMethodInfo");
                Assert.NotNull(iRuntimeMethodInfoType);

                var runtimeTypeType = Type.GetType("System.RuntimeType");
                Assert.NotNull(runtimeTypeType);

                ConstructorInfo signatureCtor = signatureType.GetConstructor(new[] { iRuntimeMethodInfoType, runtimeTypeType });
                Assert.NotNull(signatureCtor);

                ConstructorInfo fooCtor = typeof(Foo).GetConstructor(new[] { typeof(int) });
                Assert.NotNull(fooCtor);

                object fooCtorSignature = signatureCtor.Invoke(new object[] { fooCtor, typeof(Foo) });
                Assert.NotNull(fooCtorSignature);

                MethodInfo invokeMethod = typeof(Action<int>).GetMethod("Invoke");
                Assert.NotNull(invokeMethod);

                object invokeMethodSignature = signatureCtor.Invoke(new object[] { invokeMethod, typeof(Action<int>) });
                Assert.NotNull(invokeMethodSignature);

                MethodInfo compareSigMethod = signatureType.GetMethod("CompareSig", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                Assert.NotNull(compareSigMethod);

                object result = compareSigMethod.Invoke(null, new object[] { fooCtorSignature, invokeMethodSignature });
                bool equal = Assert.IsType<bool>(result);
                Assert.True(equal);
            }
        }
    }
}
