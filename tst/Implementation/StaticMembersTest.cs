using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Inspector.Implementation
{
    public class StaticMembersTest
    {
        public class Constructor: StaticMembersTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenTypeIsNullToFailFast() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new StaticMembers(null!));
                Assert.Equal("type", thrown.ParamName);
            }

            [Fact]
            public void PassesTypeToBase() {
                Type type = typeof(TestType);

                TypeMembers sut = new StaticMembers(type);

                Assert.Same(type, sut.Type);
                Assert.Null(sut.Instance);
            }

            static class TestType { }
        }

        public class Fields: StaticMembersTest
        {
            [Fact]
            public void ReturnsAllStaticFieldsDeclaredByGivenType() {
                var sut = new StaticMembers(typeof(TestType));

                IEnumerable<Field> fields = sut.Fields();

                VerifyFieldsOfTestType(fields);
            }

            [Fact]
            public void ReturnsAllStaticFieldsInheritedByGivenType() {
                var sut = new StaticMembers(typeof(TwiceDerivedType));

                IEnumerable<Field> fields = sut.Fields();

                VerifyFieldsOfTestType(fields);
            }

            void VerifyFieldsOfTestType(IEnumerable<Field> fields) {
                FieldInfo[] expected = typeof(TestType).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                Assert.Equal(6, fields.Count());
                Assert.Equal(expected, fields.Select(field => field.Info));
                fields.ToList().ForEach(field => Assert.Null(field.Instance));
            }

            class TestType
            {
                static FieldType privateField = new FieldType();
                protected static FieldType protectedField = new FieldType();
                private protected static FieldType privateProtectedField = new FieldType();
                protected internal static FieldType protectedInternalField = new FieldType();
                internal static FieldType internalField = new FieldType();
                public static FieldType publicField = new FieldType();
                public FieldType instanceField = new FieldType();
            }

            class DerivedType: TestType { }

            class TwiceDerivedType: DerivedType { }

            class FieldType { }
        }

        public class Methods: StaticMembersTest
        {
            [Fact]
            public void ReturnsAllStaticMethodsInheritedByGivenType() {
                var sut = new StaticMembers(typeof(TwiceDerivedType));

                IEnumerable<Method> methods = sut.Methods();

                VerifyMethodsOfTestType(methods);
            }

            void VerifyMethodsOfTestType(IEnumerable<Method> methods) {
                const BindingFlags allStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                HashSet<MethodInfo> expected = typeof(TestType).GetMethods(allStatic).Concat(
                    typeof(object).GetMethods(allStatic))
                    .ToHashSet();
                var actual = methods.Select(field => field.Info).ToHashSet();
                Assert.True(expected.SetEquals(actual));
                methods.ToList().ForEach(field => Assert.Null(field.Instance));
            }

            class TestType
            {
                static void PrivateMethod() { }
                protected static void ProtectedMethod() { }
                private protected static void PrivateProtectedMethod() { }
                protected internal static void ProtectedInternalMethod() { }
                internal static void InternalMethod() { }
                public static void PublicMethod() { }
                public void InstanceMethod() { }
            }

            class DerivedType: TestType { }

            class TwiceDerivedType: DerivedType { }
        }
    }
}
