using System;
using System.Reflection;
using NSubstitute;
using Xunit;
namespace Inspector
{
    public class MembersTest
    {
        readonly Members<MemberInfo, Member<MemberInfo>> sut;

        class TestType { }

        readonly Type type = typeof(TestType);
        readonly object instance = new TestType();
        readonly Members<MemberInfo, Member<MemberInfo>>.InfoProvider getMemberInfo = Substitute.For<Members<MemberInfo, Member<MemberInfo>>.InfoProvider>();
        readonly Members<MemberInfo, Member<MemberInfo>>.Factory createMember = Substitute.For<Members<MemberInfo, Member<MemberInfo>>.Factory>();

        public MembersTest() {
            sut = new Members<MemberInfo, Member<MemberInfo>>(type, instance, getMemberInfo, createMember);
        }

        public class Constructor : MembersTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(null, instance, getMemberInfo, createMember));
                Assert.Equal("type", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInfoProviderIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(type, instance, null, createMember));
                Assert.Equal("getMemberInfo", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFactoryIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(type, instance, getMemberInfo, null));
                Assert.Equal("createMember", thrown.ParamName);
            }
        }
    }
}
