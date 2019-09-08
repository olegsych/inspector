using System;
using System.Reflection;
using Inspector.Implementation;
using Xunit;

namespace Inspector
{
    public class TypeExtensionsTest
    {
        class TestClass { }

        public class AccessibilityMethod: TypeExtensionsTest
        {
            readonly Type type = typeof(TestClass);

            [Fact]
            public void InternalReturnsInternalMembersOfGivenType() {
                IMembers actual = type.Internal();
                VerifyMembers(actual, type, Accessibility.Internal);
            }

            [Fact]
            public void PrivateReturnsPrivateMembersOfGivenType() {
                IMembers actual = type.Private();
                VerifyMembers(actual, type, Accessibility.Private);
            }

            [Fact]
            public void ProtectedReturnsProtectedMembersOfGivenType() {
                IMembers actual = type.Protected();
                VerifyMembers(actual, type, Accessibility.Protected);
            }

            [Fact]
            public void PublicReturnsPublicMembersOfGivenType() {
                IMembers actual = type.Public();
                VerifyMembers(actual, type, Accessibility.Public);
            }

            static void VerifyMembers(IMembers actual, Type type, Accessibility accessibility) {
                var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
                Assert.Equal(accessibility, accessibleMembers.Accessibility);

                var staticMembers = Assert.IsType<StaticMembers>(accessibleMembers.Source);
                Assert.Same(type, staticMembers.Type);
            }
        }

        public class ConstructorMethod: ConstructorExtensionsTest
        {
            readonly Type type = typeof(TestClass);

            [Fact]
            public void ReturnsSingleConstructorOfGivenType() {
                Assert.Same(selected, type.Constructor());
                var members = Assert.IsType<Members<ConstructorInfo, Constructor>>(selection);
                Assert.Same(type, members.Type);
            }
        }

        public class New: TypeExtensionsTest
        {
            class PropertyType { }

            class TypeWithPublicConstructor
            {
                public TypeWithPublicConstructor(PropertyType value) =>
                    Property = value;

                public PropertyType Property { get; }
            }

            [Fact]
            public void ReturnsNewInstanceCreatedByPublicConstructor() {
                var value = new PropertyType();
                var instance = Assert.IsType<TypeWithPublicConstructor>(typeof(TypeWithPublicConstructor).New(value));
                Assert.Same(value, instance.Property);
            }

            class TypeWithPrivateConstructor
            {
                TypeWithPrivateConstructor(PropertyType value) =>
                    Property = value;

                public PropertyType Property { get; }
            }

            [Fact]
            public void ReturnsNewInstanceCreatedByPrivateConstructor() {
                var value = new PropertyType();
                var instance = Assert.IsType<TypeWithPrivateConstructor>(typeof(TypeWithPrivateConstructor).New(value));
                Assert.Same(value, instance.Property);
            }
        }

        public class Uninitialized: TypeExtensionsTest
        {
            class TestType
            {
                public readonly int TestField = 42;
            }

            [Fact]
            public void ReturnsUninitializedInstanceOfGivenType() {
                var instance = Assert.IsType<TestType>(typeof(TestType).Uninitialized());
                Assert.Equal(0, instance.TestField);
            }
        }
    }
}
