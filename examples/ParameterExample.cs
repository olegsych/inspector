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

            TestType(int foo, string bar, Baz baz) { }

            void TestMethod(int foo, string bar, Baz baz) { }
        }

        readonly TestType instance = Type<TestType>.Uninitialized();
        readonly Type runtimeType = typeof(TestType).GetNestedType("Baz", BindingFlags.NonPublic)!;

        public class ConstructorParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                ParameterInfo parameter = instance.Constructor().Parameter(runtimeType);
                Assert.Equal("baz", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ParameterInfo parameter = instance.Constructor().Parameter<string>();
                Assert.Equal("bar", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByName() {
                ParameterInfo parameter = instance.Constructor().Parameter("bar");
                Assert.Equal(typeof(string), parameter.ParameterType);
            }
        }

        public class ConstructorInfoParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter(runtimeType);
                Assert.Equal("baz", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter<string>();
                Assert.Equal("bar", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByName() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter("bar");
                Assert.Equal(typeof(string), parameter.ParameterType);
            }
        }

        public class MethodParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                ParameterInfo parameter = instance.Method().Parameter(runtimeType);
                Assert.Equal("baz", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ParameterInfo parameter = instance.Method().Parameter<string>();
                Assert.Equal("bar", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByName() {
                ParameterInfo parameter = instance.Method().Parameter("bar");
                Assert.Equal(typeof(string), parameter.ParameterType);
            }
        }

        public class MethodInfoParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter(runtimeType);
                Assert.Equal("baz", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter<string>();
                Assert.Equal("bar", parameter.Name);
            }

            [Fact]
            public void CanBeAccessedByName() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter("bar");
                Assert.Equal(typeof(string), parameter.ParameterType);
            }
        }
    }
}
