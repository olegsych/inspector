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
            public void ThrowsDescriptiveExceptionWhenGivenNoInstanceForInstanceConstructor() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Constructor(info));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance is required for constructor {info}", thrown.Message);
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
            public void InvokesConstructorOfGivenType() {
                var expectedField = new FieldType();
                var sut = new Constructor(info, instance);

                sut.Invoke(expectedField);

                Assert.Same(expectedField, instance.field);
            }

            [Fact]
            public void InvokesStaticConstructor() {
                StaticType.field = new FieldType();
                var sut = new Constructor(typeof(StaticType).TypeInitializer);

                sut.Invoke();

                Assert.Same(StaticType.expected, StaticType.field);
            }
        }

        public class IsStatic : ConstructorTest
        {
            [Fact]
            public void ReturnsFalseForInstanceConstructorInfo() =>
                Assert.False(new Constructor(info, instance).IsStatic);

            [Fact]
            public void ReturnsTrueForStaticConstructorInfo() =>
                Assert.True(new Constructor(typeof(InstanceType).TypeInitializer, null).IsStatic);
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
