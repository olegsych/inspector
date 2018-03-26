using System;
using System.Linq.Expressions;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest
    {
        public class FieldMethod : FieldExtensionFixture
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
    }
}
