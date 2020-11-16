using System;
using System.Reflection;
using Shouldly;
using Xunit;

namespace Inspector
{
    public class TypeAccessExample
    {
        static class Foo
        {
#pragma warning disable 414
            public static Bar barField;
#pragma warning restore 414

            public static Baz BazProperty { get; set; }

#pragma warning disable 67
            public static event EventHandler<Bar> BarEvent;
#pragma warning restore 67

            public static Baz BarFunc(Bar _) => default;

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
                field.Info.ShouldBe(typeof(Foo).GetRuntimeField(nameof(Foo.barField)));
            }

            [Fact]
            public void GetByTypeAndName() {
                Field<Bar> field = typeof(Foo).Field<Bar>(nameof(Foo.barField));
                field.Info.ShouldBe(typeof(Foo).GetRuntimeField(nameof(Foo.barField)));
            }
        }

        public class Property : TypeAccessExample
        {
            [Fact]
            public void GetByType() {
                Property<Baz> property = typeof(Foo).Property<Baz>();
                property.Info.ShouldBe(typeof(Foo).GetRuntimeProperty(nameof(Foo.BazProperty)));
            }

            [Fact]
            public void GetByTypeAndName() {
                Property<Baz> property = typeof(Foo).Property<Baz>(nameof(Foo.BazProperty));
                property.Info.ShouldBe(typeof(Foo).GetRuntimeProperty(nameof(Foo.BazProperty)));
            }
        }
    }
}
