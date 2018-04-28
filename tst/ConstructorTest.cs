using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class ConstructorTest {
        // Constructor parameters
        readonly ConstructorInfo info = typeof(InstanceType).GetConstructors().Single();
        readonly InstanceType instance = new InstanceType(new FieldType());

        public class Ctor : ConstructorTest {
            [Fact]
            public void InitializesMemberWithGivenConstructorInfoAndInstance() {
                var sut = new Constructor(info, instance);

                Assert.Same(info, sut.Info);
                Assert.Same(instance, sut.Instance);
            }

            [Fact]
            public void InitializesMemberWIthGivenStaticConstructor() {
                var sut = new Constructor(typeof(InstanceType).TypeInitializer);

                Assert.Same(typeof(InstanceType).TypeInitializer, sut.Info);
                Assert.Null(sut.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInstanceIsGivenForStaticConstructorInfo() {
                var thrown = Assert.Throws<ArgumentException>(() => new Constructor(typeof(InstanceType).TypeInitializer, instance));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith("Static constructor cannot be used with an instance.", thrown.Message);
            }
        }

        public class Create : ConstructorTest
        {
            [Fact]
            public void ReturnsConstructorInstanceWithGivenArguments() {
                Constructor result = Constructor.Create(info, instance);

                Assert.Same(info, result.Info);
                Assert.Same(instance, result.Instance);
            }
        }

        public class Invoke : ConstructorTest
        {
            [Fact]
            public void InvokesConstructorOfGivenTypeAndReturnsNull() {
                var expectedField = new FieldType();
                var sut = new Constructor(info, instance);

                object result = sut.Invoke(expectedField);

                Assert.Null(result);
                Assert.Same(expectedField, instance.field);
            }

            [Fact]
            public void InvokesConstructorOfGivenTypeAndReturnsNewInstance() {
                var expectedField = new FieldType();
                var sut = new Constructor(info);

                object result = sut.Invoke(expectedField);

                var actual = Assert.IsType<InstanceType>(result);
                Assert.Same(expectedField, actual.field);
            }

            [Fact]
            public void InvokesStaticConstructorAndReturnsNull() {
                StaticType.field = new FieldType();
                var sut = new Constructor(typeof(StaticType).TypeInitializer);

                object result = sut.Invoke();

                Assert.Null(result);
                Assert.Same(StaticType.expected, StaticType.field);
            }
        }

        class InstanceType {
            public readonly FieldType field;

            public InstanceType(FieldType parameter) =>
                field = parameter;

            static InstanceType() { }
        }

        static class StaticType
        {
            public static readonly FieldType expected = new FieldType();
            public static FieldType field;
            static StaticType() => field = expected;
        }

        class FieldType { }
    }
}
