using System;
using System.Reflection;
using Shouldly;
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
                parameter.Name.ShouldBe("baz");
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ParameterInfo parameter = instance.Constructor().Parameter<string>();
                parameter.Name.ShouldBe("bar");
            }

            [Fact]
            public void CanBeAccessedByName() {
                ParameterInfo parameter = instance.Constructor().Parameter("bar");
                parameter.ParameterType.ShouldBe(typeof(string));
            }
        }

        public class ConstructorInfoParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter(runtimeType);
                parameter.Name.ShouldBe("baz");
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter<string>();
                parameter.Name.ShouldBe("bar");
            }

            [Fact]
            public void CanBeAccessedByName() {
                ConstructorInfo constructor = instance.Constructor();
                ParameterInfo parameter = constructor.Parameter("bar");
                parameter.ParameterType.ShouldBe(typeof(string));
            }
        }

        public class MethodParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                ParameterInfo parameter = instance.Method().Parameter(runtimeType);
                parameter.Name.ShouldBe("baz");
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                ParameterInfo parameter = instance.Method().Parameter<string>();
                parameter.Name.ShouldBe("bar");
            }

            [Fact]
            public void CanBeAccessedByName() {
                ParameterInfo parameter = instance.Method().Parameter("bar");
                parameter.ParameterType.ShouldBe(typeof(string));
            }
        }

        public class MethodInfoParameter: ParameterExample
        {
            [Fact]
            public void CanBeAccessedByRuntimeType() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter(runtimeType);
                parameter.Name.ShouldBe("baz");
            }

            [Fact]
            public void CanBeAccessedByCompileTimeType() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter<string>();
                parameter.Name.ShouldBe("bar");
            }

            [Fact]
            public void CanBeAccessedByName() {
                MethodInfo method = instance.Method();
                ParameterInfo parameter = method.Parameter("bar");
                parameter.ParameterType.ShouldBe(typeof(string));
            }
        }
    }
}
