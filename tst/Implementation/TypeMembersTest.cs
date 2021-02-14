using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector.Implementation
{
    public class TypeMembersTest
    {
        class TestType { }

        public class Constructors: TypeMembersTest
        {
            [Theory, MemberData(nameof(ConstructorData))]
            internal void ReturnsConstructorsOfGivenTypeOrInstance(IMembers sut, Type type, object? instance) {
                var members = (Members<ConstructorInfo, Constructor>)sut.Constructors();

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetConstructors, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Constructor.Create, members.CreateMember);
                Assert.Equal(Lifetime.Instance, members.Lifetime);
            }

            public static IEnumerable<object?[]> ConstructorData() {
                var instance = new TestType();
                yield return new object?[] { Substitute.ForPartsOf<TypeMembers>(instance), instance.GetType(), instance };
                yield return new object?[] { Substitute.ForPartsOf<TypeMembers>(typeof(TestType)), typeof(TestType), null };
            }
        }

        public class Events: TypeMembersTest
        {
            [Theory, MemberData(nameof(MemberData))]
            internal void ReturnsEventsOfGivenTypeOrInstance(IMembers sut, Type type, object? instance, Lifetime lifetime) {
                var members = (Members<EventInfo, Event>)sut.Events();

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetEvents, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Event.Create, members.CreateMember);
                Assert.Equal(lifetime, members.Lifetime);
            }
        }

        public class Fields: TypeMembersTest
        {
            [Theory, MemberData(nameof(MemberData))]
            internal void ReturnsFieldsOfGivenTypeOrInstance(IMembers sut, Type type, object? instance, Lifetime lifetime) {
                var members = (Members<FieldInfo, Field>)sut.Fields();

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetFields, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Field.Create, members.CreateMember);
                Assert.Equal(lifetime, members.Lifetime);
            }
        }

        public class Methods: TypeMembersTest
        {
            [Theory, MemberData(nameof(MemberData))]
            internal void ReturnsMethodsOfGivenTypeOrInstance(IMembers sut, Type type, object? instance, Lifetime lifetime) {
                var members = (Members<MethodInfo, Method>)sut.Methods();

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetMethods, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Method.Create, members.CreateMember);
                Assert.Equal(lifetime, members.Lifetime);
            }
        }

        public class Properties: TypeMembersTest
        {
            [Theory, MemberData(nameof(MemberData))]
            internal void ReturnsPropertiesOfGivenTypeOrInstance(IMembers sut, Type type, object? instance, Lifetime lifetime) {
                var members = (Members<PropertyInfo, Property>)sut.Properties();

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetProperties, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Property.Create, members.CreateMember);
                Assert.Equal(lifetime, members.Lifetime);
            }
        }

        public static IEnumerable<object?[]> MemberData() {
            var instance = new TestType();
            yield return new object?[] { Substitute.ForPartsOf<TypeMembers>(instance), instance.GetType(), instance, Lifetime.Instance };
            yield return new object?[] { Substitute.ForPartsOf<TypeMembers>(typeof(TestType)), typeof(TestType), null, Lifetime.Static };
        }
    }
}
