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
        readonly FieldInfo info = typeof(InstanceType).GetFields().Single(_ => _.FieldType == typeof(FieldType));
        readonly object instance = new InstanceType();

        public GenericFieldTest() =>
            sut = new Field<FieldType>(info, instance);

        public class Constructor : GenericFieldTest
        {
            [Fact]
            public void InitializesBaseWithGivenArgument() {
                Field field = sut;
                Assert.Same(info, field.Info);
                Assert.Same(instance, field.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInfoDoesNotHaveExpectedFieldType() {
                FieldInfo unexpected = typeof(InstanceType).GetFields().Single(_ => _.FieldType != typeof(FieldType));
                var thrown = Assert.Throws<ArgumentException>(() => new Field<FieldType>(unexpected, instance));
                Assert.Equal("info", thrown.ParamName);
                Assert.StartsWith($"Field type {unexpected.FieldType} doesn't match expected {typeof(FieldType)}.", thrown.Message);
            }
        }

        public class Get : GenericFieldTest
        {
            [Fact]
            public void GetsFieldValue() {
                FieldType value = sut.Get();
                Assert.Same(((InstanceType)instance).field, value);
            }
        }

        public class Set : GenericFieldTest
        {
            [Fact]
            public void SetsFieldValue() {
                var value = new FieldType();
                sut.Set(value);
                Assert.Same(value, ((InstanceType)instance).field);
            }
        }

        public class Value : GenericFieldTest
        {
            [Fact]
            public void GetsFieldValue() {
                FieldType value = sut.Value;
                Assert.Same(((InstanceType)instance).field, value);
            }

            [Fact]
            public void SetsFieldValue() {
                var value = new FieldType();
                sut.Value = value;
                Assert.Same(value, ((InstanceType)instance).field);
            }
        }

        public class ImplicitOperatorT : GenericFieldTest
        {
            [Fact]
            public void ImplicitlyConvertsFieldToValueType() {
                FieldType value = sut;
                Assert.Same(value, ((InstanceType)instance).field);
            }

            [Fact]
            public void ConvertsNullFieldToDefaultValueToSupportImplicitTypeConversionRules() {
                FieldType value = (Field<FieldType>)null;
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
