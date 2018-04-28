using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class TypeScopeTest
    {
        readonly TypeScope sut;

        readonly object instance = new TestType();
        readonly System.Type instanceType;
        readonly TypeInfo instanceTypeInfo;

        class TestType { }

        public TypeScopeTest() {
            sut = Substitute.ForPartsOf<TypeScope>(instance);
            instanceType = instance.GetType();
            instanceTypeInfo = instanceType.GetTypeInfo();
        }

        public class GetFields : TypeScopeTest
        {
            new readonly IFilter<Field> sut;

            public GetFields() =>
                sut = base.sut;

            [Fact]
            public void ReturnsFieldsOfGivenInstance() {
                var actual = Assert.IsType<Members<FieldInfo, Field>>(sut.Get());
                Assert.Same(instanceType, actual.Type);
                Assert.Same(instance, actual.Instance);
                Assert.Equal(instanceTypeInfo.GetFields, actual.GetMemberInfo(instanceTypeInfo));
                Assert.Equal(Field.Create, actual.CreateMember);
            }
        }

        public class GetMethods : TypeScopeTest
        {
            new readonly IFilter<Method> sut;

            public GetMethods() =>
                sut = base.sut;

            [Fact]
            public void ReturnsMethodsOfGivenInstance() {
                var actual = Assert.IsType<Members<MethodInfo, Method>>(sut.Get());
                Assert.Same(instanceType, actual.Type);
                Assert.Same(instance, actual.Instance);
                Assert.Equal(instanceTypeInfo.GetMethods, actual.GetMemberInfo(instanceTypeInfo));
                Assert.Equal(Method.Create, actual.CreateMember);
            }
        }
    }
}
