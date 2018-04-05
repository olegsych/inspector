using System;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class FieldTest
    {
        readonly Field sut;

        // Constructor parameters
        readonly FieldInfo instanceField = typeof(TestType).GetField(nameof(TestType.Field));
        readonly FieldInfo staticField = typeof(TestType).GetField(nameof(TestType.StaticField));
        readonly object instance = new TestType();

        public FieldTest() =>
            sut = new Field(instanceField, instance);

        public class Constructor : FieldTest
        {
            [Fact]
            public void InitializesNewInstanceForInstanceField() {
                Member<FieldInfo> member = sut;

                Assert.Same(instanceField, member.Info);
                Assert.Same(instance, member.Instance);
            }

            [Fact]
            public void InitializesNewInstanceForStaticField() {
                Member<FieldInfo> member = new Field(staticField, null);

                Assert.Same(staticField, member.Info);
                Assert.Null(member.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenGivenNoInstanceForInstanceField() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Field(instanceField, null));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance is required for field {instanceField.Name}", thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenGivenInstanceForStaticField() {
                var thrown = Assert.Throws<ArgumentException>(() => new Field(staticField, instance));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance shouldn't be specified for static field {staticField.Name}.", thrown.Message);
            }
        }

        public class Value : FieldTest
        {
            [Fact]
            public void GetsFieldValue() {
                object value = sut.Value;
                Assert.Same(((TestType)instance).Field, value);
            }

            [Fact]
            public void SetsFieldValue() {
                object value = new FieldType();
                sut.Value = value;
                Assert.Same(value, ((TestType)instance).Field);
            }
        }

        public class Get : FieldTest
        {
            [Fact]
            public void GetsFieldValue() {
                object value = sut.Get();
                Assert.Same(((TestType)instance).Field, value);
            }
        }

        public class Set : FieldTest
        {
            [Fact]
            public void SetsFieldValue() {
                object value = new FieldType();
                sut.Set(value);
                Assert.Same(value, ((TestType)instance).Field);
            }
        }

        class TestType
        {
            public FieldType Field = new FieldType();
            public static FieldType StaticField = new FieldType();
        }

        class FieldType { }
    }
}
