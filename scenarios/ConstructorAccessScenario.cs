using System.Reflection;
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
        }
    }
}
