using System;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class ObjectAccessExample
    {
        public class AccessibleTypes : ObjectAccessExample
        {
            class Bar { }
            class Baz { }

            public class Create : AccessibleTypes
            {
                class Foo
                {
                    Bar barField;
                    Baz bazField;

                    Foo() : this(new Bar(), new Baz()) { }

                    Foo(Bar bar, Baz baz) {
                        barField = bar;
                        bazField = baz;
                    }
                }

                [Fact]
                public void NewInstanceWithDefaultConstructor() {
                    Foo foo = Type<Foo>.New();

                    Assert.NotNull(foo.Field<Bar>().Value);
                    Assert.NotNull(foo.Field<Baz>().Value);
                }

                [Fact]
                public void NewInstanceWithGivenConstructorParameters() {
                    var bar = new Bar();
                    var baz = new Baz();

                    Foo foo = Type<Foo>.New(bar, baz);

                    Assert.Same(bar, foo.Field<Bar>().Value);
                    Assert.Same(baz, foo.Field<Baz>().Value);
                }

                [Fact]
                public void UninitializedInstance() {
                    Foo foo = Type<Foo>.Uninitialized();

                    Assert.Null(foo.Field<Bar>().Value);
                    Assert.Null(foo.Field<Baz>().Value);
                }
            }

            public class AccessFields : ObjectAccessExample
            {
                class Foo
                {
                    Bar bar = new Bar();
                    Baz baz1 = new Baz();
                    Baz baz2 = new Baz();
                }

                readonly Foo foo = new Foo();

                [Fact]
                public void ByType() {
                    Field<Bar> field = foo.Field<Bar>();
                }

                [Fact]
                public void ByTypeAndName() {
                    Field<Baz> field = foo.Field<Baz>("baz1");
                }
            }

            public class AccessActions : AccessibleTypes
            {
                class Foo
                {
                    void BarAction(Bar bar) { }

                    void BazAction1(Baz baz) { }
                    void BazAction2(Baz baz) { }

                    void OutAction1(out Bar bar) => throw new NotImplementedException();
                    void OutAction2(out Bar bar) => throw new NotImplementedException();

                    Bar BarFunc(Baz baz) => throw new NotImplementedException();
                }

                readonly Foo foo = new Foo();

                [Fact]
                public void SimpleActionWithUniqueParameters() {
                    Action<Bar> action = foo.Method<Action<Bar>>();
                }

                [Fact]
                public void SimpleActionWithUniqueName() {
                    Action<Baz> action = foo.Method<Action<Baz>>("BazAction1");
                }

                [Fact]
                public void SimpleFuncWithUniqueParameters() {
                    Func<Baz, Bar> fun = foo.Method<Func<Baz, Bar>>();
                }

                delegate void OutAction(out Bar bar);

                [Fact]
                public void MethodWithAdvancedParameters() {
                    OutAction action = foo.Method<OutAction>("OutAction1");
                }
            }
        }

        public class InaccessibleTypes : ObjectAccessExample
        {
            class Inaccessible
            {
                class Foo
                {
                    Bar barField;
                    Baz bazField;

                    Foo() : this(new Bar(), new Baz()) { }

                    Foo(Bar bar, Baz baz) {
                        barField = bar;
                        bazField = baz;
                    }
                }

                class Bar { }
                class Baz { }
            }

            static readonly Type typeOfFoo = typeof(Inaccessible).GetNestedType("Foo", BindingFlags.NonPublic)!;
            static readonly Type typeOfBar = typeof(Inaccessible).GetNestedType("Bar", BindingFlags.NonPublic)!;
            static readonly Type typeOfBaz = typeof(Inaccessible).GetNestedType("Baz", BindingFlags.NonPublic)!;

            public class Create : InaccessibleTypes
            {
                [Fact]
                public void NewInstanceWithDefaultConstructor() {
                    object foo = typeOfFoo.New();

                    Assert.NotNull(foo.Field(typeOfBar).Value);
                    Assert.NotNull(foo.Field(typeOfBaz).Value);
                }

                [Fact]
                public void NewInstanceWithGivenConstructorParameters() {
                    object bar = typeOfBar.New();
                    object baz = typeOfBaz.New();

                    object foo = typeOfFoo.New(bar, baz);

                    Assert.Same(bar, foo.Field(typeOfBar).Value);
                    Assert.Same(baz, foo.Field(typeOfBaz).Value);
                }

                [Fact]
                public void UninitializedInstance() {
                    object foo = typeOfFoo.Uninitialized();

                    Assert.Null(foo.Field(typeOfBar).Value);
                    Assert.Null(foo.Field(typeOfBaz).Value);
                }
            }

            public class AccessFields : InaccessibleTypes
            {
                readonly object foo = Activator.CreateInstance(typeOfFoo, true)!;

                [Fact]
                public void ByType() {
                    Field field = foo.Field(typeOfBar);

                    Assert.IsType(typeOfBar, field.Value);
                }

                [Fact]
                public void ByName() {
                    Field field = foo.Field("barField");

                    Assert.IsType(typeOfBar, field.Value);
                }

                [Fact]
                public void ByTypeAndName() {
                    Field field = foo.Field(typeOfBar, "barField");

                    Assert.IsType(typeOfBar, field.Value);
                }
            }
        }
    }
}
