using System;
using System.Reflection;
using Shouldly;
using Xunit;

namespace Inspector
{
    public class PropertyAccessScenario
    {
        class Foo
        {
            Bar Bar { get; set; }
            public Foo(Bar bar) => Bar = bar;
        }

        class Bar { }

        [Fact]
        public void GetValueExplicitly() {
            var bar = new Bar();
            var foo = new Foo(bar);

            Bar value = foo.Property<Bar>().Get();

            value.ShouldBeSameAs(bar);
        }

        [Fact]
        public void GetValueImplicitly() {
            var bar = new Bar();
            var foo = new Foo(bar);

            Bar value = foo.Property<Bar>();

            value.ShouldBeSameAs(bar);
        }

        [Fact]
        public void SetValueExplicitly() {
            var bar = new Bar();
            var foo = new Foo(null);

            foo.Property<Bar>().Set(bar);

            foo.Property<Bar>().Get().ShouldBe(bar);
        }

        [Fact]
        public void GetInfoExplicitly() {
            var bar = new Bar();
            var foo = new Foo(bar);

            PropertyInfo info = foo.Property<Bar>().Info;

            info.ShouldBe(typeof(Foo).GetRuntimeProperty("bar"));
        }

        [Fact]
        public void GetInfoImplicitly() {
            var bar = new Bar();
            var foo = new Foo(bar);

            PropertyInfo info = foo.Property<Bar>();

            info.ShouldBe(typeof(Foo).GetRuntimeProperty("bar"));
        }

        public class ReadOnlyPropertyBackedByField
        {
            class Foo { }

            class Bar
            {
                Foo Foo { get; }
                public Bar(Foo foo) => Foo = foo;
            }

            [Fact]
            public void CanBeSet() {
                var foo = new Foo();
                var bar = new Bar(null);

                bar.Property<Foo>().Set(foo);

                bar.Property<Foo>().Get().ShouldBe(foo);
            }
        }

        public class ReadOnlyPropertyNotBackedByField
        {
            class Foo
            {
                public Bar BarProperty => new Bar();
            }

            class Bar { }

            [Fact]
            public void CannotBeSetAndWillThrowDescriptiveException() {
                var foo = new Foo();

                var thrown = Should.Throw<InvalidOperationException>(() => foo.Field<Bar>().Set(new Bar()));

                thrown.Message.ShouldContain(nameof(Bar));
                thrown.Message.ShouldContain(nameof(Foo.BarProperty));
            }
        }
    }
}
