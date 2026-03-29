using System;
using System.Reflection;
using Xunit;

namespace Inspector
{
    public class ParameterExample
    {
        class TestType
        {
            class Baz { }

            TestType(int min, int max, string label, Baz extra) { }

            void TestMethod(int min, int max, string label, Baz extra) { }
        }

        readonly TestType instance = Type<TestType>.Uninitialized();
        readonly Type runtimeType = typeof(TestType).GetNestedType("Baz", BindingFlags.NonPublic)!;

        public class ConstructorParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                ParameterInfo parameter = instance.Constructor().Parameter(runtimeType);
                Assert.Equal("extra", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ParameterInfo parameter = instance.Constructor().Parameter<string>();
                Assert.Equal("label", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByName() {
                ParameterInfo parameter = instance.Constructor().Parameter("label");
                Assert.Equal(typeof(string), parameter.ParameterType);
            }

            [Fact]
            public void CanBeAccessedByPosition() {
                ParameterInfo parameter = instance.Constructor().Parameter(2);
                Assert.Equal("label", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeTypeAndPosition() {
                ParameterInfo parameter = instance.Constructor().Parameter<int>(1);
                Assert.Equal("max", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByRuntimeTypeAndPosition() {
                ParameterInfo parameter = instance.Constructor().Parameter(typeof(int), 0);
                Assert.Equal("min", parameter.Name);
            }
        }

        public class ConstructorInfoParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter(runtimeType);
                Assert.Equal("extra", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter<string>();
                Assert.Equal("label", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByName() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter("label");
                Assert.Equal(typeof(string), parameter.ParameterType);
            }

            [Fact]
            public void CanBeAccessedByPosition() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter(2);
                Assert.Equal("label", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeTypeAndPosition() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter<int>(1);
                Assert.Equal("max", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByRuntimeTypeAndPosition() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter(typeof(int), 0);
                Assert.Equal("min", parameter.Name);
            }
        }

        public class MethodParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                ParameterInfo parameter = instance.Method().Parameter(runtimeType);
                Assert.Equal("extra", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ParameterInfo parameter = instance.Method().Parameter<string>();
                Assert.Equal("label", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByName() {
                ParameterInfo parameter = instance.Method().Parameter("label");
                Assert.Equal(typeof(string), parameter.ParameterType);
            }

            [Fact]
            public void CanBeAccessedByPosition() {
                ParameterInfo parameter = instance.Method().Parameter(2);
                Assert.Equal("label", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeTypeAndPosition() {
                ParameterInfo parameter = instance.Method().Parameter<int>(1);
                Assert.Equal("max", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByRuntimeTypeAndPosition() {
                ParameterInfo parameter = instance.Method().Parameter(typeof(int), 0);
                Assert.Equal("min", parameter.Name);
            }
        }

        public class MethodInfoParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter(runtimeType);
                Assert.Equal("extra", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter<string>();
                Assert.Equal("label", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByName() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter("label");
                Assert.Equal(typeof(string), parameter.ParameterType);
            }

            [Fact]
            public void CanBeAccessedByPosition() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter(2);
                Assert.Equal("label", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeTypeAndPosition() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter<int>(1);
                Assert.Equal("max", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByRuntimeTypeAndPosition() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter(typeof(int), 0);
                Assert.Equal("min", parameter.Name);
            }
        }
    }
}
