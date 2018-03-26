using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class IScopeExtensionsTest
    {
        public class FieldMethod : FieldSelectorFixture
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();
            readonly string fieldName = Guid.NewGuid().ToString();
            readonly Type fieldType = typeof(FieldValue);

            // Shared test fixture
            readonly TestType instance = new TestType();
            readonly Field selected;
            IFilter<Field> selection;

            public FieldMethod() {
                selected = new Field(typeof(TestType).GetField(nameof(TestType.Field)), instance);
                select.Invoke(Arg.Do<IFilter<Field>>(f => selection = f)).Returns(selected);
            }

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

            class TestType
            {
                public FieldValue Field = new FieldValue();
            }

            class FieldValue { }
        }
    }
}
