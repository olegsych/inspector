using System;
using System.Linq.Expressions;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest
    {
        public class FieldMethod : FieldSelectorFixture
        {
            // Method parameters
            readonly object instance = new TestType();
            readonly Type fieldType = typeof(FieldValue);
            readonly string fieldName = Guid.NewGuid().ToString();

            // Test fixture
            readonly Field selected;
            IFilter<Field> selection;

            public FieldMethod() {
                selected = new Field(typeof(TestType).GetField(nameof(TestType.Field)), instance);
                select.Invoke(Arg.Do<IFilter<Field>>(f => selection = f)).Returns(selected);
            }

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

            class TestType
            {
                public FieldValue Field = new FieldValue();
            }

            class FieldValue { }
        }
    }
}
