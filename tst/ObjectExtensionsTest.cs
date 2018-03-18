using System;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class ObjectExtensionsTest
    {
        public class FieldTest : ObjectExtensionsTest
        {
            [Fact]
            public void ReturnsPrivateFieldOfGivenIstanceAndFieldType() {
                var instance = new TypeWithPrivateField();
                FieldInfo info = instance.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(_ => _.FieldType == typeof(FieldType));

                Field<FieldType> field = instance.Field<FieldType>();

                VerifyField(instance, info, field);
            }

            [Fact]
            public void ReturnsPublicFieldOfGivenIstanceAndFieldType() {
                var instance = new TypeWithPublicField();
                FieldInfo expectedInfo = instance.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Single(_ => _.FieldType == typeof(FieldType));

                Field<FieldType> field = instance.Field<FieldType>();

                VerifyField(instance, expectedInfo, field);
            }

            [Fact]
            public void ReturnsFieldOfInstanceDynamicallyGeneratedByCastleProxy() {
                var instance = Substitute.ForPartsOf<AbstractType>();
                FieldInfo expectedInfo = instance.GetType().BaseType
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(_ => _.FieldType == typeof(FieldType));

                Field<FieldType> field = instance.Field<FieldType>();

                VerifyField(instance, expectedInfo, field);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInstanceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => default(object).Field<FieldType>());
                Assert.Equal("instance", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInstanceDoesNotHaveFieldOfGivenType() {
                var instance = new TypeWithPublicField();
                var thrown = Assert.Throws<ArgumentException>(() => instance.Field<UnexpectedFieldType>());
                Assert.Equal("T", thrown.ParamName);
                Assert.StartsWith($"{instance.GetType()} doesn't have instance fields of type {typeof(UnexpectedFieldType)}.", thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionsWhenInstanceHasMoreThanOneFieldOfGivenType() {
                var instance = new TypeWithMultipleFields();
                var thrown = Assert.Throws<ArgumentException>(() => instance.Field<FieldType>());
                Assert.Equal("T", thrown.ParamName);
                Assert.StartsWith($"{instance.GetType()} has more than one instance field of type {typeof(FieldType)}.", thrown.Message);
            }

            static void VerifyField(object expectedInstance, FieldInfo expectedInfo, Field<FieldType> field) {
                object actualInstance = typeof(Field<FieldType>)
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(_ => _.FieldType == typeof(object))
                    .GetValue(field);
                var actualInfo = (FieldInfo)typeof(Field<FieldType>)
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Single(_ => _.FieldType == typeof(FieldInfo))
                    .GetValue(field);
                Assert.Same(expectedInstance, actualInstance);
                Assert.Same(expectedInfo, actualInfo);
            }

            public class FieldType { }

            class UnexpectedFieldType { }

            class TypeWithPublicField
            {
#pragma warning disable 649 // public field used only via reflection
                public FieldType field;
#pragma warning restore 649
            }

#pragma warning disable 169 // private fields used only via reflection

            public class AbstractType
            {
                FieldType field;
            }

            class TypeWithPrivateField
            {
                FieldType field;
            }

            class TypeWithMultipleFields
            {
                FieldType field1;
                FieldType field2;
            }

#pragma warning restore 169
        }
    }
}
