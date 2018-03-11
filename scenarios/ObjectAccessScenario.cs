using System;
using System.Reflection;
using Shouldly;
using Xunit;

namespace Inspector
{
    public class ObjectAccessScenario
    {
        public class AccessibleTypes : ObjectAccessScenario
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

                    foo.Field<Bar>().Value.ShouldNotBeNull();
                    foo.Field<Baz>().Value.ShouldNotBeNull();
                }

                [Fact]
                public void NewInstanceWithGivenConstructorParameters() {
                    var bar = new Bar();
                    var baz = new Baz();

                    Foo foo = Type<Foo>.New(bar, baz);

                    foo.Field<Bar>().Value.ShouldBeSameAs(bar);
                    foo.Field<Baz>().Value.ShouldBeSameAs(baz);
                }

                [Fact]
                public void UninitializedInstance() {
                    Foo foo = Type<Foo>.Uninitialized();

                    foo.Field<Bar>().Value.ShouldBeNull();
                    foo.Field<Baz>().Value.ShouldBeNull();
                }
            }

            public class AccessFields : ObjectAccessScenario
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
                    Action<Bar> action = foo.Action<Bar>();
                }

                [Fact]
                public void SimpleActionWithUniqueName() {
                    Action<Baz> action = foo.Action<Baz>("BazAction1");
                }

                [Fact]
                public void SimpleFuncWithUniqueParameters() {
                    Func<Baz, Bar> fun = foo.Func<Baz, Bar>();
                }

                delegate void OutAction(out Bar bar);

                [Fact]
                public void MethodWithAdvancedParameters() {
                    OutAction action = foo.Method<OutAction>("OutAction1");
                }
            }
        }

        public class InaccessibleTypes : ObjectAccessScenario
        {
            class Inaccessible
            {
                class Foo
                {
                    Bar barField;
                    Baz bazField;

                    Foo() : this (new Bar(), new Baz()) { }

                    Foo(Bar bar, Baz baz) {
                        barField = bar;
                        bazField = baz;
                    }
                }

                class Bar { }
                class Baz { }
            }

            static readonly Type typeOfFoo = typeof(Inaccessible).GetNestedType("Foo", BindingFlags.NonPublic);
            static readonly Type typeOfBar = typeof(Inaccessible).GetNestedType("Bar", BindingFlags.NonPublic);
            static readonly Type typeOfBaz = typeof(Inaccessible).GetNestedType("Baz", BindingFlags.NonPublic);

            public class Create : InaccessibleTypes
            {
                [Fact]
                public void NewInstanceWithDefaultConstructor() {
                    object foo = typeOfFoo.New();

                    foo.Field(typeOfBar).Value.ShouldNotBeNull();
                    foo.Field(typeOfBaz).Value.ShouldNotBeNull();
                }

                [Fact]
                public void NewInstanceWithGivenConstructorParameters() {
                    object bar = typeOfBar.New();
                    object baz = typeOfBaz.New();

                    object foo = typeOfFoo.New(bar, baz);

                    foo.Field(typeOfBar).Value.ShouldBeSameAs(bar);
                    foo.Field(typeOfBaz).Value.ShouldBeSameAs(baz);
                }

                [Fact]
                public void UninitializedInstance() {
                    object foo = typeOfFoo.Uninitialized();

                    foo.Field(typeOfBar).Value.ShouldBeNull();
                    foo.Field(typeOfBaz).Value.ShouldBeNull();
                }
            }

            public class AccessFields : InaccessibleTypes
            {
                readonly object foo = typeOfFoo.New();

                [Fact]
                public void ByType() {
                    Field field = foo.Field(typeOfBar);

                    field.Value.ShouldBeOfType(typeOfBar);
                }

                [Fact]
                public void ByName() {
                    Field field = foo.Field("barField");

                    field.Value.ShouldBeOfType(typeOfBar);
                }

                [Fact]
                public void ByTypeAndName() {
                    Field field = foo.Field(typeOfBar, "barField");

                    field.Value.ShouldBeOfType(typeOfBar);
                }
            }
        }
    }
}
