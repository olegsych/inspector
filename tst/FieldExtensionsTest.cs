using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests of extension methods that return <see cref="Field"/> and <see cref="Field{T}"/>.
    /// </summary>
    [Collection(nameof(FieldExtensionsTest))]
    public class FieldExtensionsTest : SelectorFixture<Field>
    {
        // Method parameters
        protected readonly Type fieldType = typeof(FieldValue);
        protected readonly string fieldName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Field selected;
        protected IFilter<Field> selection;

        public FieldExtensionsTest() {
            selected = new Field(typeof(TestType).GetField(nameof(TestType.Field)), instance);
            select.Invoke(Arg.Do<IFilter<Field>>(f => selection = f)).Returns(selected);
        }

        protected static void VerifyGenericField<T>(Field selected, Field<T> generic) {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        internal static FieldNameFilter VerifyFilter(IFilter<Field> selection, string fieldName) {
            var filter = Assert.IsType<FieldNameFilter>(selection);
            Assert.Equal(fieldName, filter.FieldName);
            return filter;
        }

        internal static FieldTypeFilter VerifyFilter(IFilter<Field> selection, Type expectedFieldType) {
            var filter = Assert.IsType<FieldTypeFilter>(selection);
            Assert.Equal(expectedFieldType, filter.FieldType);
            return filter;
        }

        protected class TestType
        {
            public FieldValue Field = new FieldValue();
        }

        protected class FieldValue { }

        public class IScopeExtension : FieldExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void ReturnsSingleFieldInGivenScope() {
                Assert.Same(selected, scope.Field());

                Assert.Same(scope, selection);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, scope.Field(fieldName));

                FieldNameFilter filter = VerifyFilter(selection, fieldName);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, scope.Field(fieldType));

                FieldTypeFilter filter = VerifyFilter(selection, fieldType);
                Assert.Same(scope, filter.Previous);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, scope.Field(fieldType, fieldName));

                FieldNameFilter nameFilter = VerifyFilter(selection, fieldName);
                FieldTypeFilter typeFilter = VerifyFilter(nameFilter.Previous, fieldType);
                Assert.Same(scope, typeFilter.Previous);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenType() {
                Field<FieldValue> generic = scope.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typeFilter = VerifyFilter(selection, typeof(FieldValue));
                Assert.Same(scope, typeFilter.Previous);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = scope.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                FieldNameFilter nameFilter = VerifyFilter(selection, fieldName);
                FieldTypeFilter typeFilter = VerifyFilter(nameFilter.Previous, fieldType);
                Assert.Same(scope, typeFilter.Previous);
            }
        }

        public class ObjectExtension : FieldExtensionsTest
        {
            [Fact]
            public void ReturnsSingleFieldInGivenType() {
                Assert.Same(selected, instance.Field());

                VerifyScope(selection, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, instance.Field(fieldType));

                FieldTypeFilter named = VerifyFilter(selection, fieldType);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, instance.Field(fieldName));

                FieldNameFilter named = VerifyFilter(selection, fieldName);
                VerifyScope(named.Previous, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, instance.Field(fieldType, fieldName));

                FieldNameFilter named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Previous, fieldType);
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericFieldOfGivenType() {
                Field<FieldValue> generic = instance.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typed = VerifyFilter(selection, typeof(FieldValue));
                VerifyScope(typed.Previous, instance);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = instance.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                FieldNameFilter named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Previous, typeof(FieldValue));
                VerifyScope(typed.Previous, instance);
            }

            static void VerifyScope(IFilter<Field> filter, object instance) {
                var scope = Assert.IsType<InstanceScope>(filter);
                Assert.Same(instance, scope.Instance);
            }
        }

        public class TypeExtension : FieldExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSingleFieldInGivenType() {
                Assert.Same(selected, testType.Field());

                VerifyScope(selection, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, testType.Field(fieldType));

                FieldTypeFilter typed = VerifyFilter(selection, fieldType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, testType.Field(fieldName));

                FieldNameFilter named = VerifyFilter(selection, fieldName);
                VerifyScope(named.Previous, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, testType.Field(fieldType, fieldName));

                FieldNameFilter named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Previous, fieldType);
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsGenericFieldOfGivenType() {
                Field<FieldValue> generic = testType.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typed = VerifyFilter(selection, typeof(FieldValue));
                VerifyScope(typed.Previous, testType);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = testType.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                FieldNameFilter named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Previous, typeof(FieldValue));
                VerifyScope(typed.Previous, testType);
            }

            static void VerifyScope(IFilter<Field> selection, Type expected) {
                var scope = Assert.IsType<StaticScope>(selection);
                Assert.Same(expected, scope.Type);
            }
        }
    }
}