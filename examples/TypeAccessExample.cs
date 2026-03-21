using System;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class TypeAccessExample
    {
        static class Foo
        {
#pragma warning disable 414
            public static Bar? barField;
#pragma warning restore 414

            public static Baz? BazProperty { get; set; }

#pragma warning disable 67
            public static event EventHandler<Bar>? BarEvent;
#pragma warning restore 67

            public static Baz? BarFunc(Bar _) => default;

            public static void BarAction(Bar _) { }

            static Foo() => barField = default;
        }

        class Bar { }

        class Baz { }

        public class Field : TypeAccessExample
        {
            [Fact]
            public void GetByType() {
                Field<Bar> field = typeof(Foo).Field<Bar>();
                Assert.Equal(typeof(Foo).GetRuntimeField(nameof(Foo.barField)), field.Info);
            }

            [Fact]
            public void GetByTypeAndName() {
                Field<Bar> field = typeof(Foo).Field<Bar>(nameof(Foo.barField));
                Assert.Equal(typeof(Foo).GetRuntimeField(nameof(Foo.barField)), field.Info);
            }
        }

        public class Property : TypeAccessExample
        {
            [Fact]
            public void GetByType() {
                Property<Baz> property = typeof(Foo).Property<Baz>();
                Assert.Equal(typeof(Foo).GetRuntimeProperty(nameof(Foo.BazProperty)), property.Info);
            }

            [Fact]
            public void GetByTypeAndName() {
                Property<Baz> property = typeof(Foo).Property<Baz>(nameof(Foo.BazProperty));
                Assert.Equal(typeof(Foo).GetRuntimeProperty(nameof(Foo.BazProperty)), property.Info);
            }
        }
    }
}
