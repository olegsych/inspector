using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests of extension methods that return <see cref="Field"/> and <see cref="Field{T}"/>.
    /// </summary>
    [Collection(nameof(FieldExtensionsTest))]
    public class FieldExtensionsTest: SelectorFixture<Field>
    {
        // Method parameters
        protected readonly Type fieldType = typeof(FieldValue);
        protected readonly string fieldName = Guid.NewGuid().ToString();

        // Shared test fixture
        protected readonly object instance = new TestType();
        protected readonly Field selected;
        protected IEnumerable<Field> selection;

        public FieldExtensionsTest() {
            selected = new Field(typeof(TestType).GetField(nameof(TestType.Field)), instance);
            select.Invoke(Arg.Do<IEnumerable<Field>>(f => selection = f)).Returns(selected);
        }

        protected static void VerifyGenericField<T>(Field selected, Field<T> generic) {
            Assert.Same(selected.Info, generic.Info);
            Assert.Same(selected.Instance, generic.Instance);
        }

        internal static MemberNameFilter<Field, FieldInfo> VerifyFilter(IEnumerable<Field> selection, string fieldName) {
            var filter = Assert.IsType<MemberNameFilter<Field, FieldInfo>>(selection);
            Assert.Equal(fieldName, filter.MemberName);
            return filter;
        }

        internal static FieldTypeFilter VerifyFilter(IEnumerable<Field> selection, Type expectedFieldType) {
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
