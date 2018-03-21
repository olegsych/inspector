using System;
using System.Reflection;
using Shouldly;
using Xunit;

namespace Inspector
{
    public class FieldAccessScenario
    {
        class Foo
        {
            readonly Bar bar;
            public Foo(Bar bar) => this.bar = bar;
        }

        class Bar { }

        // Shared test fixture
        readonly Foo foo;
        readonly Bar bar = new Bar();
        public FieldAccessScenario() => foo = new Foo(bar);

        public class FieldValue : FieldAccessScenario
        {
            [Fact]
            public void GetWithMethod() {
                Bar value = foo.Field<Bar>().Get();
                value.ShouldBeSameAs(bar);
            }

            [Fact]
            public void GetWithProperty() {
                Bar value = foo.Field<Bar>().Value;
                value.ShouldBeSameAs(bar);
            }

            [Fact]
            public void GetWithImplicitConversionToFieldType() {
                Bar value = foo.Field<Bar>();
                value.ShouldBeSameAs(bar);
            }

            [Fact]
            public void SetWithMethod() {
                var baz = new Bar();
                foo.Field<Bar>().Set(baz);
                foo.Field<Bar>().Get().ShouldBe(baz);
            }

            [Fact]
            public void SetWithValue() {
                var baz = new Bar();
                foo.Field<Bar>().Value = baz;
                foo.Field<Bar>().Value.ShouldBe(baz);
            }

            //[Fact]
            //public void CompareImplicitly() {
            //    (foo.Field<Bar>() == bar).ShouldBeTrue();
            //    (foo.Field<Bar>() != bar).ShouldBeFalse();
            //    (bar == foo.Field<Bar>()).ShouldBeTrue();
            //    (bar != foo.Field<Bar>()).ShouldBeFalse();
            //}
        }

        public class FieldInfoScenario : FieldAccessScenario
        {
            [Fact]
            public void GetWithProperty() {
                FieldInfo info = foo.Field<Bar>().Info;
                info.ShouldBe(typeof(Foo).GetRuntimeField("bar"));
            }

            [Fact]
            public void GetWithImplicitConversionToFieldInfo() {
                FieldInfo info = foo.Field<Bar>();
                info.ShouldBe(typeof(Foo).GetRuntimeField("bar"));
            }
        }

        public class Operators
        {
            class Foo
            {
                int bar;
                public Foo(int bar) => this.bar = bar;
            }

            readonly Foo foo = new Foo(42);

            [Fact]
            public void UseFieldValueWithBinaryOperators() {
                (foo.Field<int>() + 1).ShouldBe(43);
            }

            [Fact]
            public void ChangeFieldValueWithAssignmentOperator() {
                foo.Field<int>().Value += 1;
                foo.Field<int>().Value.ShouldBe(43);
            }
        }

        public class FilterByVisibility
        {
            class Foo
            {
#pragma warning disable 169
                Bar privateField;
#pragma warning disable 169

#pragma warning disable 649
                protected Bar protectedField;
                internal Bar internalField;
                public readonly Bar publicField;
                protected internal Bar protectedInternalField;
                private protected Bar privateProtectedField;
#pragma warning restore 649
            }

            readonly Foo foo = new Foo();

            [Fact]
            public void SelectPrivateField() {
                FieldInfo field = foo.Private().Field<Bar>();
                field.Name.ShouldBe("privateField");
            }

            [Fact]
            public void SelectProtectedField() {
                FieldInfo field = foo.Protected().Field<Bar>();
                field.Name.ShouldBe("protectedField");
            }

            [Fact]
            public void SelectInternalField() {
                FieldInfo field = foo.Internal().Field<Bar>();
                field.Name.ShouldBe("protectedField");
            }

            [Fact]
            public void SelectProtectedInternalField() {
                FieldInfo field = foo.Protected().Internal().Field<Bar>();
                field.Name.ShouldBe("protectedInternalField");
            }

            [Fact]
            public void SelectPrivateProtectedField() {
                FieldInfo field = foo.Private().Protected().Field<Bar>();
                field.Name.ShouldBe("privateProtectedField");
            }

            [Fact]
            public void SelectPublicField() {
                FieldInfo field = foo.Public().Field<Bar>();
                field.Name.ShouldBe("publicField");
            }

            [Fact]
            public void ThrowDescriptiveExceptionWhenCompbinationOfVisibilityFiltersIsInvalid() {
                Should.Throw<InvalidOperationException>(() => foo.Public().Private().Field<Bar>());
                Should.Throw<InvalidOperationException>(() => foo.Public().Internal().Field<Bar>());
                Should.Throw<InvalidOperationException>(() => foo.Public().Protected().Field<Bar>());
            }
        }

        public class FilterByDeclaringType
        {
#pragma warning disable 649

            class Foo
            {
                public Baz fooField;
            }

            class Bar : Foo
            {
                public Baz barField;
            }

#pragma warning restore 649

            class Baz { }

            readonly Bar bar = new Bar();

            [Fact]
            public void ThrowDescriptiveExceptionWhenMoreThanOneFieldOfGivenTypeExists() {
                var thrown = Should.Throw<InvalidOperationException>(() => bar.Field<Baz>());
                thrown.Message.ShouldContain(typeof(Baz).FullName);
                thrown.Message.ShouldContain(typeof(Foo).FullName);
                thrown.Message.ShouldContain(typeof(Bar).FullName);
            }

            [Fact]
            public void SelectDeclaredField() {
                FieldInfo field = bar.Declared().Field<Baz>();
                field.DeclaringType.ShouldBe(typeof(Bar));
            }

            [Fact]
            public void SelectInheritedField() {
                FieldInfo field = bar.Inherited().Field<Baz>();
                field.DeclaringType.ShouldBe(typeof(Foo));
            }

            [Fact]
            public void SelectFieldInheritedFromSpecificType() {
                FieldInfo field = bar.Inherited<Bar>().Field<Baz>();
                field.DeclaringType.ShouldBe(typeof(Bar));
            }

            [Fact]
            public void SelectFieldDeclaredInSpecificTypeWithSpecificVisibility() {
                FieldInfo field = bar.Public().Inherited<Bar>().Field<Baz>();
                field.DeclaringType.ShouldBe(typeof(Bar));
            }
        }

        public class FilterByName
        {
#pragma warning disable 649

            class Foo
            {
                public Qux field1;
                public Qux field2;
            }

            class Bar : Foo
            {
                public new Qux field1;
                public new Qux field2;
            }

            class Baz : Bar
            {
                public new Qux field1;
                public new Qux field2;
            }

#pragma warning restore 649

            class Qux { }

            readonly Foo foo = new Foo();
            readonly Bar bar = new Bar();
            readonly Baz baz = new Baz();

            [Fact]
            public void ThrowDescriptiveExceptionWhenMoreThanOneFieldOfGivenTypeExistsInDeclaringType() {
                var thrown = Should.Throw<InvalidOperationException>(() => foo.Field<Qux>());
                thrown.Message.ShouldContain(typeof(Qux).FullName);
                thrown.Message.ShouldContain(typeof(Foo).FullName);
                thrown.Message.ShouldContain(nameof(Foo.field1));
                thrown.Message.ShouldContain(nameof(Foo.field2));
            }

            [Fact]
            public void SelectFieldDeclaredWithSpecificName() {
                FieldInfo field = foo.Field<Qux>(nameof(Foo.field2));
                field.Name.ShouldBe(nameof(Foo.field2));
            }

            [Fact]
            public void SelectFieldWithSpecificNameAndVisibility() {
                FieldInfo field = foo.Public().Field<Qux>(nameof(Foo.field2));
                field.Name.ShouldBe(nameof(Foo.field2));
            }

            [Fact]
            public void SelectInheritedFieldWithSpecificName() {
                FieldInfo field = bar.Inherited<Foo>().Field<Qux>(nameof(Foo.field2));
                field.DeclaringType.ShouldBe(typeof(Foo));
                field.Name.ShouldBe(nameof(Foo.field2));
            }

            [Fact]
            public void SelectFieldWithSpecificNameAndDeclaringType() {
                FieldInfo field = baz.Declared<Bar>().Field<Qux>(nameof(Bar.field2));
                field.DeclaringType.ShouldBe(typeof(Bar));
                field.Name.ShouldBe(nameof(Bar.field2));
            }
        }
    }
}
