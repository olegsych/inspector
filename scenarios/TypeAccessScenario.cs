using System;
using System.Reflection;
using Shouldly;
using Xunit;

namespace Inspector
{
    public class TypeAccessScenario
    {
        static class Foo
        {
#pragma warning disable 414
            public static Bar barField;
#pragma warning restore 414

            public static Bar BarProperty { get; set; }

#pragma warning disable 67
            public static event EventHandler<Bar> BarEvent;
#pragma warning restore 67

            public static Baz BarFunc(Bar _) => default;

            public static void BarAction(Bar _) { }

            static Foo() => barField = default;
        }

        class Bar { }

        class Baz { }

        public class Field : TypeAccessScenario
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

        public class Property : TypeAccessScenario
        {
            [Fact]
            public void GetByType() {
                Property<Bar> property = typeof(Foo).Property<Bar>();
                property.Info.ShouldBe(typeof(Foo).GetRuntimeProperty(nameof(Foo.BarProperty)));
            }

            [Fact]
            public void GetByTypeAndName() {
                Property<Bar> property = typeof(Foo).Property<Bar>(nameof(Foo.BarProperty));
                property.Info.ShouldBe(typeof(Foo).GetRuntimeProperty(nameof(Foo.BarProperty)));
            }
        }
    }
}
