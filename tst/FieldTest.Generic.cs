using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class GenericFieldTest
    {
        readonly Field<FieldType> sut;

        // Constructor parameters
        readonly Field field;

        // Test fixture
        readonly InstanceType instance = new InstanceType();

        public GenericFieldTest() {
            FieldInfo info = typeof(InstanceType).GetFields().Single(_ => _.FieldType == typeof(FieldType));
            field = new Field(info, instance);

            sut = new Field<FieldType>(field);
        }

        public class Constructor: GenericFieldTest
        {
            [Fact]
            public void InitializesBaseWithGivenArgument() {
                Field @base = sut;
                Assert.Same(field.Info, @base.Info);
                Assert.Same(field.Instance, @base.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFieldIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Field<FieldType>(null!));
                Assert.Equal("field", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInfoDoesNotHaveExpectedFieldType() {
                FieldInfo unexpected = typeof(InstanceType).GetFields().Single(_ => _.FieldType != typeof(FieldType));
                var thrown = Assert.Throws<ArgumentException>(() => new Field<FieldType>(new Field(unexpected, new InstanceType())));
                Assert.Equal("field", thrown.ParamName);
                Assert.StartsWith($"Field type {unexpected.FieldType} doesn't match expected {typeof(FieldType)}.", thrown.Message);
            }
        }

        public class Get: GenericFieldTest
        {
            [Fact]
            public void GetsFieldValue() {
                FieldType value = sut.Get();
                Assert.Same(instance.field, value);
            }
        }

        public class Set: GenericFieldTest
        {
            [Fact]
            public void SetsFieldValue() {
                var value = new FieldType();
                sut.Set(value);
                Assert.Same(value, instance.field);
            }
        }

        public class Value: GenericFieldTest
        {
            [Fact]
            public void GetsFieldValue() {
                FieldType value = sut.Value;
                Assert.Same(instance.field, value);
            }

            [Fact]
            public void SetsFieldValue() {
                var value = new FieldType();
                sut.Value = value;
                Assert.Same(value, instance.field);
            }
        }

        public class ImplicitOperatorT: GenericFieldTest
        {
            [Fact]
            public void ImplicitlyConvertsFieldToValueType() {
                FieldType value = sut;
                Assert.Same(value, instance.field);
            }

            [Fact]
            public void ConvertsNullFieldToDefaultValueToSupportImplicitTypeConversionRules() {
                Field<FieldType>? @null = null;
                FieldType? value = @null;
                Assert.Null(value);
            }
        }

        class InstanceType
        {
            public FieldType field = new FieldType();
            public string anotherField = string.Empty;
        }

        class FieldType { }
    }
}
