using System.Reflection;
using Xunit;

namespace Inspector
{
    public class FieldTest
    {
        readonly Field sut;

        // Constructor parameters
        readonly FieldInfo instanceField = typeof(TestType).GetField(nameof(TestType.Field))!;
        readonly FieldInfo staticField = typeof(TestType).GetField(nameof(TestType.StaticField))!;
        readonly object instance = new TestType();

        public FieldTest() =>
            sut = new Field(instanceField, instance);

        public class Constructor: FieldTest
        {
            [Fact]
            public void InitializesNewInstanceForInstanceField() {
                Member<FieldInfo> member = sut;

                Assert.Same(instanceField, member.Info);
                Assert.Same(instance, member.Instance);
            }
        }

        public class Create: FieldTest
        {
            [Fact]
            public void ReturnsFieldWithGivenFieldInfoAndInstance() {
                Field actual = Field.Create(instanceField, instance);

                Assert.Same(instanceField, actual.Info);
                Assert.Same(instance, actual.Instance);
            }
        }

        public class Value: FieldTest
        {
            [Fact]
            public void GetsFieldValue() {
                object? value = sut.Value;
                Assert.Same(((TestType)instance).Field, value);
            }

            [Fact]
            public void SetsFieldValue() {
                object value = new FieldType();
                sut.Value = value;
                Assert.Same(value, ((TestType)instance).Field);
            }
        }

        public class Get: FieldTest
        {
            [Fact]
            public void GetsFieldValue() {
                object? value = sut.Get();
                Assert.Same(((TestType)instance).Field, value);
            }
        }

        public class Set: FieldTest
        {
            [Fact]
            public void SetsFieldValue() {
                object value = new FieldType();
                sut.Set(value);
                Assert.Same(value, ((TestType)instance).Field);
            }
        }

        public class IsStatic: FieldTest
        {
            [Fact]
            public void ReturnsFalseForInstanceFieldInfo() =>
                Assert.False(new Field(instanceField, instance).IsStatic);

            [Fact]
            public void ReturnsTrueForStaticFieldInfo() =>
                Assert.True(new Field(staticField, null).IsStatic);
        }

        class TestType
        {
            public FieldType Field = new FieldType();
            public static FieldType StaticField = new FieldType();
        }

        class FieldType { }
    }
}
