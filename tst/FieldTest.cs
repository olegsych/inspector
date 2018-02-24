using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class FieldTest
    {
        readonly Field<FieldType> sut;

        // Constructor parameters
        readonly FieldInfo info = typeof(InstanceType).GetFields().Single(_ => _.FieldType == typeof(FieldType));
        readonly object instance = new InstanceType();

        public FieldTest() => sut = new Field<FieldType>(info, instance);

        public class Constructor : FieldTest
        {
            [Fact]
            public void InitializesInfoWithGivenArgument()
            {
                var actual = (FieldInfo)sut.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(_ => _.FieldType == typeof(FieldInfo))
                    .GetValue(sut);
                Assert.Same(info, actual);
            }

            [Fact]
            public void InitializesInstanceWithGivenArgument()
            {
                object actual = sut.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(_ => _.FieldType == typeof(object))
                    .GetValue(sut);
                Assert.Same(instance, actual);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInfoIsNullToFailFast()
            {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Field<FieldType>(null, instance));
                Assert.Equal("info", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInfoDoesNotHaveExpectedFieldType()
            {
                FieldInfo unexpected = typeof(InstanceType).GetFields().Single(_ => _.FieldType != typeof(FieldType));
                var thrown = Assert.Throws<ArgumentException>(() => new Field<FieldType>(unexpected, instance));
                Assert.Equal("info", thrown.ParamName);
                Assert.StartsWith($"Field type {unexpected.FieldType} doesn't match expected {typeof(FieldType)}.", thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInstanceIsNullToFailFast()
            {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Field<FieldType>(info, null));
                Assert.Equal("instance", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInstanceTypeDoesNotMatchDeclaringTypeOfField()
            {
                object unexpected = new AnotherType();
                var thrown = Assert.Throws<ArgumentException>(() => new Field<FieldType>(info, unexpected));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance type {unexpected.GetType()} doesn't match type {info.DeclaringType} where field is declared.", thrown.Message);
            }
        }

        public class Value : FieldTest
        {
            [Fact]
            public void GetsFieldValue()
            {
                var expected = new FieldType();
                ((InstanceType)instance).field = expected;

                FieldType actual = sut.Value;

                Assert.Same(expected, actual);
            }

            [Fact]
            public void SetsFieldValue()
            {
                var expected = new FieldType();

                sut.Value = expected;

                FieldType actual = ((InstanceType)instance).field;
                Assert.Same(expected, actual);
            }
        }

        #pragma warning disable 649 // Unassigned fields are accessed via Reflection

        class InstanceType
        {
            public FieldType field;
            public string anotherField;
        }

        class AnotherType
        {
            public FieldType field;
        }

        #pragma warning restore 649

        class FieldType { }
    }
}
