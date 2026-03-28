using System;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class FieldAccessExample
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
        public FieldAccessExample() => foo = new Foo(bar);

        public class FieldValue : FieldAccessExample
        {
            [Fact]
            public void GetWithMethod() {
                Bar? value = foo.Field<Bar>().Get();
                Assert.Same(bar, value);
            }

            [Fact]
            public void GetWithProperty() {
                Bar? value = foo.Field<Bar>().Value;
                Assert.Same(bar, value);
            }

            [Fact]
            public void GetWithImplicitConversionToFieldType() {
                Bar? value = foo.Field<Bar>();
                Assert.Same(bar, value);
            }

            [Fact]
            public void SetWithMethod() {
                var baz = new Bar();
                foo.Field<Bar>().Set(baz);
                Assert.Equal(baz, foo.Field<Bar>().Get());
            }

            [Fact]
            public void SetWithValue() {
                var baz = new Bar();
                foo.Field<Bar>().Value = baz;
                Assert.Equal(baz, foo.Field<Bar>().Value);
            }

            //[Fact]
            //public void CompareImplicitly() {
            //    (foo.Field<Bar>() == bar).ShouldBeTrue();
            //    (foo.Field<Bar>() != bar).ShouldBeFalse();
            //    (bar == foo.Field<Bar>()).ShouldBeTrue();
            //    (bar != foo.Field<Bar>()).ShouldBeFalse();
            //}
        }

        public class FieldInfoScenario : FieldAccessExample
        {
            [Fact]
            public void GetWithProperty() {
                FieldInfo info = foo.Field<Bar>().Info;
                Assert.Equal(typeof(Foo).GetField("bar", BindingFlags.Instance | BindingFlags.NonPublic), info);
            }

            [Fact]
            public void GetWithImplicitConversionToFieldInfo() {
                FieldInfo info = foo.Field<Bar>();
                Assert.Equal(typeof(Foo).GetField("bar", BindingFlags.Instance | BindingFlags.NonPublic), info);
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
                Assert.Equal(43, foo.Field<int>() + 1);
            }

            [Fact]
            public void ChangeFieldValueWithAssignmentOperator() {
                foo.Field<int>().Value += 1;
                Assert.Equal(43, foo.Field<int>().Value);
            }
        }

        public class FilterByVisibility
        {
            class Foo
            {
#pragma warning disable 169
                Bar? privateField;
#pragma warning disable 169

#pragma warning disable 649
                protected Bar? protectedField;
                internal Bar? internalField;
                public readonly Bar? publicField;
                protected internal Bar? protectedInternalField;
                private protected Bar? privateProtectedField;
#pragma warning restore 649
            }

            readonly Foo foo = new Foo();

            [Fact]
            public void SelectPrivateField() {
                FieldInfo field = foo.Private().Field<Bar>();
                Assert.Equal("privateField", field.Name);
            }

            [Fact]
            public void SelectProtectedField() {
                FieldInfo field = foo.Protected().Field<Bar>();
                Assert.Equal("protectedField", field.Name);
            }

            [Fact]
            public void SelectInternalField() {
                FieldInfo field = foo.Internal().Field<Bar>();
                Assert.Equal("internalField", field.Name);
            }

            [Fact]
            public void SelectProtectedInternalField() {
                FieldInfo field = foo.Protected().Internal().Field<Bar>();
                Assert.Equal("protectedInternalField", field.Name);
            }

            [Fact]
            public void SelectPrivateProtectedField() {
                FieldInfo field = foo.Private().Protected().Field<Bar>();
                Assert.Equal("privateProtectedField", field.Name);
            }

            [Fact]
            public void SelectPublicField() {
                FieldInfo field = foo.Public().Field<Bar>();
                Assert.Equal("publicField", field.Name);
            }

            [Fact]
            public void ThrowDescriptiveExceptionWhenCompbinationOfVisibilityFiltersIsInvalid() {
                Assert.Throws<InvalidOperationException>(() => foo.Public().Private().Field<Bar>());
                Assert.Throws<InvalidOperationException>(() => foo.Public().Internal().Field<Bar>());
                Assert.Throws<InvalidOperationException>(() => foo.Public().Protected().Field<Bar>());
            }
        }

        public class FilterByDeclaringType
        {
#pragma warning disable 649

            class Foo
            {
                public Baz? fooField;
            }

            class Bar : Foo
            {
                public Baz? barField;
            }

#pragma warning restore 649

            class Baz { }

            readonly Bar bar = new Bar();

            [Fact]
            public void ThrowDescriptiveExceptionWhenMoreThanOneFieldOfGivenTypeExists() {
                var thrown = Assert.Throws<InvalidOperationException>(() => bar.Field<Baz>());
                Assert.Contains(typeof(Baz).FullName!, thrown.Message);
                Assert.Contains(typeof(Foo).FullName!, thrown.Message);
                Assert.Contains(typeof(Bar).FullName!, thrown.Message);
            }

            [Fact]
            public void SelectDeclaredField() {
                FieldInfo field = bar.Declared().Field<Baz>();
                Assert.Equal(typeof(Bar), field.DeclaringType);
            }

            [Fact]
            public void SelectFieldDeclaredBySpecificType() {
                FieldInfo field = bar.DeclaredBy<Foo>().Field<Baz>();
                Assert.Equal(typeof(Foo), field.DeclaringType);
            }

            [Fact]
            public void SelectInheritedField() {
                FieldInfo field = bar.Inherited().Field<Baz>();
                Assert.Equal(typeof(Foo), field.DeclaringType);
            }

            [Fact]
            public void SelectFieldInheritedFromSpecificType() {
                FieldInfo field = bar.InheritedFrom<Foo>().Field<Baz>();
                Assert.Equal(typeof(Foo), field.DeclaringType);
            }
        }

        public class FilterByName
        {
#pragma warning disable 649

            class Foo
            {
                public Qux? field1;
                public Qux? field2;
            }

            class Bar : Foo
            {
                public new Qux? field1;
                public new Qux? field2;
            }

            class Baz : Bar
            {
                public new Qux? field1;
                public new Qux? field2;
            }

#pragma warning restore 649

            class Qux { }

            readonly Foo foo = new Foo();
            readonly Bar bar = new Bar();
            readonly Baz baz = new Baz();

            [Fact]
            public void ThrowDescriptiveExceptionWhenMoreThanOneFieldOfGivenTypeExistsInDeclaringType() {
                var thrown = Assert.Throws<InvalidOperationException>(() => foo.Field<Qux>());
                Assert.Contains(typeof(Qux).FullName!, thrown.Message);
                Assert.Contains(typeof(Foo).FullName!, thrown.Message);
                Assert.Contains(nameof(Foo.field1), thrown.Message);
                Assert.Contains(nameof(Foo.field2), thrown.Message);
            }

            [Fact]
            public void SelectFieldDeclaredWithSpecificName() {
                FieldInfo field = foo.Field<Qux>(nameof(Foo.field2));
                Assert.Equal(nameof(Foo.field2), field.Name);
            }

            [Fact]
            public void SelectFieldWithSpecificNameAndVisibility() {
                FieldInfo field = foo.Public().Field<Qux>(nameof(Foo.field2));
                Assert.Equal(nameof(Foo.field2), field.Name);
            }

            [Fact]
            public void SelectInheritedFieldWithSpecificName() {
                FieldInfo field = bar.InheritedFrom<Foo>().Field<Qux>(nameof(Foo.field2));
                Assert.Equal(typeof(Foo), field.DeclaringType);
                Assert.Equal(nameof(Foo.field2), field.Name);
            }

            [Fact]
            public void SelectFieldWithSpecificNameAndDeclaringType() {
                FieldInfo field = baz.DeclaredBy<Bar>().Field<Qux>(nameof(Bar.field2));
                Assert.Equal(typeof(Bar), field.DeclaringType);
                Assert.Equal(nameof(Bar.field2), field.Name);
            }
        }
    }
}
