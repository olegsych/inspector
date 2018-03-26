using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests that need to substitute static <see cref="Selector{Field}.Select"/> method.
    /// </summary>
    [Collection(nameof(FieldSelectorFixture))]
    public class FieldSelectorFixture : SelectorFixture<Field>
    {
        // Method parameters
        protected readonly Type fieldType = typeof(FieldValue);
        protected readonly string fieldName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Field selected;
        protected IFilter<Field> selection;

        public FieldSelectorFixture() {
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
    }
}
