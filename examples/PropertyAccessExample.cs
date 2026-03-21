using System;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class PropertyAccessExample
    {
        class Foo
        {
            Bar? Bar { get; set; }
            public Foo(Bar? bar) => Bar = bar;
        }

        class Bar { }

        [Fact]
        public void GetValueExplicitly() {
            var bar = new Bar();
            var foo = new Foo(bar);

            Bar? value = foo.Property<Bar>().Get();

            Assert.Same(bar, value);
        }

        [Fact]
        public void GetValueImplicitly() {
            var bar = new Bar();
            var foo = new Foo(bar);

            Bar? value = foo.Property<Bar>();

            Assert.Same(bar, value);
        }

        [Fact]
        public void SetValueExplicitly() {
            var bar = new Bar();
            var foo = new Foo(null);

            foo.Property<Bar>().Set(bar);

            Assert.Equal(bar, foo.Property<Bar>().Get());
        }

        [Fact]
        public void GetInfoExplicitly() {
            var bar = new Bar();
            var foo = new Foo(bar);

            PropertyInfo info = foo.Property<Bar>().Info;

            Assert.Equal(typeof(Foo).GetProperty("Bar", BindingFlags.Instance | BindingFlags.NonPublic), info);
        }

        [Fact]
        public void GetInfoImplicitly() {
            var bar = new Bar();
            var foo = new Foo(bar);

            PropertyInfo info = foo.Property<Bar>();

            Assert.Equal(typeof(Foo).GetProperty("Bar", BindingFlags.Instance | BindingFlags.NonPublic), info);
        }

        public class ReadOnlyPropertyBackedByField
        {
            class Foo { }

            class Bar
            {
                Foo? Foo { get; }
                public Bar(Foo? foo) => Foo = foo;
            }

            [Fact]
            public void CanBeSet() {
                var foo = new Foo();
                var bar = new Bar(null);

                bar.Property<Foo>().Set(foo);

                Assert.Equal(foo, bar.Property<Foo>().Get());
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

                var thrown = Assert.Throws<InvalidOperationException>(() => foo.Field<Bar>().Set(new Bar()));

                Assert.Contains(nameof(Bar), thrown.Message);
                Assert.Contains(nameof(Foo.BarProperty), thrown.Message);
            }
        }
    }
}
