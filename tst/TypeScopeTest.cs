using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class TypeScopeTest
    {
        class TestType { }

        public class GetConstructors: TypeScopeTest
        {
            [Theory, MemberData(nameof(ConstructorData))]
            internal void ReturnsConstructorsOfGivenTypeOrInstance(IFilter<Constructor> sut, Type type, object instance) {
                var members = Assert.IsType<Members<ConstructorInfo, Constructor>>(sut.Get());

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetConstructors, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Constructor.Create, members.CreateMember);
                Assert.Equal(Lifetime.Instance, members.Lifetime);
            }

            public static IEnumerable<object[]> ConstructorData() {
                var instance = new TestType();
                yield return new object[] { Substitute.ForPartsOf<TypeScope>(instance), instance.GetType(), instance };
                yield return new object[] { Substitute.ForPartsOf<TypeScope>(typeof(TestType)), typeof(TestType), null };
            }
        }

        public class GetEvents: TypeScopeTest
        {
            [Theory, MemberData(nameof(MemberData))]
            internal void ReturnsEventsOfGivenTypeOrInstance(IFilter<Event> sut, Type type, object instance, Lifetime lifetime) {
                var members = Assert.IsType<Members<EventInfo, Event>>(sut.Get());

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetEvents, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Event.Create, members.CreateMember);
                Assert.Equal(lifetime, members.Lifetime);
            }
        }

        public class GetFields: TypeScopeTest
        {
            [Theory, MemberData(nameof(MemberData))]
            internal void ReturnsFieldsOfGivenTypeOrInstance(IFilter<Field> sut, Type type, object instance, Lifetime lifetime) {
                var members = Assert.IsType<Members<FieldInfo, Field>>(sut.Get());

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetFields, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Field.Create, members.CreateMember);
                Assert.Equal(lifetime, members.Lifetime);
            }
        }

        public class GetMethods: TypeScopeTest
        {
            [Theory, MemberData(nameof(MemberData))]
            internal void ReturnsMethodsOfGivenTypeOrInstance(IFilter<Method> sut, Type type, object instance, Lifetime lifetime) {
                var members = Assert.IsType<Members<MethodInfo, Method>>(sut.Get());

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetMethods, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Method.Create, members.CreateMember);
                Assert.Equal(lifetime, members.Lifetime);
            }
        }

        public class GetProperties: TypeScopeTest
        {
            [Theory, MemberData(nameof(MemberData))]
            internal void ReturnsPropertiesOfGivenTypeOrInstance(IFilter<Property> sut, Type type, object instance, Lifetime lifetime) {
                var members = Assert.IsType<Members<PropertyInfo, Property>>(sut.Get());

                Assert.Same(type, members.Type);
                Assert.Same(instance, members.Instance);
                Assert.Equal(type.GetTypeInfo().GetProperties, members.GetMemberInfo(type.GetTypeInfo()));
                Assert.Equal(Property.Create, members.CreateMember);
                Assert.Equal(lifetime, members.Lifetime);
            }
        }

        public static IEnumerable<object[]> MemberData() {
            var instance = new TestType();
            yield return new object[] { Substitute.ForPartsOf<TypeScope>(instance), instance.GetType(), instance, Lifetime.Instance };
            yield return new object[] { Substitute.ForPartsOf<TypeScope>(typeof(TestType)), typeof(TestType), null, Lifetime.Static };
        }
    }
}
