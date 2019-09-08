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

        public class IMembersExtension: FieldExtensionsTest
        {
            // Method parameters
            readonly IMembers members = Substitute.For<IMembers>();

            // Arrange
            readonly IEnumerable<Field> fields = Substitute.For<IEnumerable<Field>>();

            public IMembersExtension() =>
                members.Fields().Returns(fields);

            [Fact]
            public void ReturnsSingleField() {
                Assert.Same(selected, members.Field());
                Assert.Same(fields, selection);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, members.Field(fieldName));
                MemberNameFilter<Field, FieldInfo> filter = VerifyFilter(selection, fieldName);
                Assert.Same(fields, filter.Source);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, members.Field(fieldType));
                FieldTypeFilter filter = VerifyFilter(selection, fieldType);
                Assert.Same(fields, filter.Source);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, members.Field(fieldType, fieldName));
                MemberNameFilter<Field, FieldInfo> nameFilter = VerifyFilter(selection, fieldName);
                FieldTypeFilter typeFilter = VerifyFilter(nameFilter.Source, fieldType);
                Assert.Same(fields, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenType() {
                Field<FieldValue> generic = members.Field<FieldValue>();
                VerifyGenericField(selected, generic);
                FieldTypeFilter typeFilter = VerifyFilter(selection, typeof(FieldValue));
                Assert.Same(fields, typeFilter.Source);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = members.Field<FieldValue>(fieldName);
                VerifyGenericField(selected, generic);
                MemberNameFilter<Field, FieldInfo> nameFilter = VerifyFilter(selection, fieldName);
                FieldTypeFilter typeFilter = VerifyFilter(nameFilter.Source, fieldType);
                Assert.Same(fields, typeFilter.Source);
            }
        }

        public class ObjectExtension: FieldExtensionsTest
        {
            [Fact]
            public void ReturnsSingleFieldInGivenType() {
                Assert.Same(selected, instance.Field());

                VerifyInstanceMembers(selection, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, instance.Field(fieldType));

                FieldTypeFilter named = VerifyFilter(selection, fieldType);
                VerifyInstanceMembers(named.Source, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, instance.Field(fieldName));

                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                VerifyInstanceMembers(named.Source, instance);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, instance.Field(fieldType, fieldName));

                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Source, fieldType);
                VerifyInstanceMembers(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericFieldOfGivenType() {
                Field<FieldValue> generic = instance.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typed = VerifyFilter(selection, typeof(FieldValue));
                VerifyInstanceMembers(typed.Source, instance);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = instance.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Source, typeof(FieldValue));
                VerifyInstanceMembers(typed.Source, instance);
            }

            static void VerifyInstanceMembers(IEnumerable<Field> filter, object instance) {
                var fields = Assert.IsType<Members<FieldInfo, Field>>(filter);
                Assert.Same(instance, fields.Instance);
            }
        }

        public class TypeExtension: FieldExtensionsTest
        {
            // Method parameters
            readonly Type testType = typeof(TestType);

            [Fact]
            public void ReturnsSingleFieldInGivenType() {
                Assert.Same(selected, testType.Field());

                VerifyStaticMembers(selection, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenType() {
                Assert.Same(selected, testType.Field(fieldType));

                FieldTypeFilter typed = VerifyFilter(selection, fieldType);
                VerifyStaticMembers(typed.Source, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenName() {
                Assert.Same(selected, testType.Field(fieldName));

                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                VerifyStaticMembers(named.Source, testType);
            }

            [Fact]
            public void ReturnsFieldWithGivenTypeAndName() {
                Assert.Same(selected, testType.Field(fieldType, fieldName));

                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Source, fieldType);
                VerifyStaticMembers(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericFieldOfGivenType() {
                Field<FieldValue> generic = testType.Field<FieldValue>();

                VerifyGenericField(selected, generic);
                FieldTypeFilter typed = VerifyFilter(selection, typeof(FieldValue));
                VerifyStaticMembers(typed.Source, testType);
            }

            [Fact]
            public void ReturnsGenericFieldWithGivenTypeAndName() {
                Field<FieldValue> generic = testType.Field<FieldValue>(fieldName);

                VerifyGenericField(selected, generic);
                MemberNameFilter<Field, FieldInfo> named = VerifyFilter(selection, fieldName);
                FieldTypeFilter typed = VerifyFilter(named.Source, typeof(FieldValue));
                VerifyStaticMembers(typed.Source, testType);
            }

            static void VerifyStaticMembers(IEnumerable<Field> selection, Type expected) {
                var fields = Assert.IsType<Members<FieldInfo, Field>>(selection);
                Assert.Same(expected, fields.Type);
            }
        }
    }
}
