using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
namespace Inspector
{
    public class InstanceScopeTest
    {
        public class Constructor: InstanceScopeTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenInstanceIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new InstanceScope(null));
                Assert.Equal("instance", thrown.ParamName);
            }

            [Fact]
            public void InitializesInstanceProperty() {
                var instance = new TestType();

                TypeScope sut = new InstanceScope(instance);

                Assert.Same(instance.GetType(), sut.Type);
                Assert.Same(instance, sut.Instance);
            }

            class TestType { }
        }

        public class GetFields: InstanceScopeTest
        {
            [Fact]
            public void ReturnsAllFieldsDeclaredByGivenInstance() {
                var instance = new TestType();
                IFilter<Field> sut = new InstanceScope(instance);

                IEnumerable<Field> fields = sut.Get();

                VerifyFieldsOfTestType(instance, fields);
            }

            [Fact]
            public void ReturnsAllFieldsInheritedByGivenInstanceType() {
                var instance = new TwiceDerivedType();
                IFilter<Field> sut = new InstanceScope(instance);

                IEnumerable<Field> fields = sut.Get();

                VerifyFieldsOfTestType(instance, fields);
            }

            void VerifyFieldsOfTestType(TestType instance, IEnumerable<Field> fields) {
                FieldInfo[] expected = typeof(TestType).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                Assert.Equal(6, fields.Count());
                Assert.Equal(expected, fields.Select(field => field.Info));
                fields.ToList().ForEach(field => Assert.Same(instance, field.Instance));
            }

            class TestType
            {
                FieldType privateField = new FieldType();
                protected FieldType protectedField = new FieldType();
                private protected FieldType privateProtectedField = new FieldType();
                protected internal FieldType protectedInternalField = new FieldType();
                internal FieldType internalField = new FieldType();
                public FieldType publicField = new FieldType();
                public static FieldType staticField = new FieldType();
            }

            class DerivedType: TestType { }

            class TwiceDerivedType: DerivedType { }

            class FieldType { }
        }

        public class GetMethods: InstanceScopeTest
        {
            [Fact]
            public void ReturnsAllMethodsDeclaredAndInheritedByGivenInstanceType() {
                var instance = new TwiceDerivedType();
                IFilter<Method> sut = new InstanceScope(instance);

                IEnumerable<Method> methods = sut.Get();

                VerifyMethodsOfTestType(instance, methods);
            }

            void VerifyMethodsOfTestType(TestType instance, IEnumerable<Method> methods) {
                var declaredInstanceMethods = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
                HashSet<MethodInfo> expected = typeof(TestType).GetMethods(declaredInstanceMethods)
                    .Concat(typeof(object).GetMethods(declaredInstanceMethods))
                    .ToHashSet();
                HashSet<MethodInfo> actual = methods.Select(method => method.Info).ToHashSet();
                Assert.True(expected.SetEquals(actual));

                methods.ToList().ForEach(method => Assert.Same(instance, method.Instance));
            }

            class TestType
            {
                void PrivateMethod() { }
                protected void ProtectedMethod() { }
                private protected void PrivateProtectedMethod() { }
                protected internal void ProtectedInternalMethod() { }
                internal void InternalMethod() { }
                public void PublicMethod() { }
                public static void StaticMethod() { }
            }

            class DerivedType: TestType { }

            class TwiceDerivedType: DerivedType { }
        }
    }
}
