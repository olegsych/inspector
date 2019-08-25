using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector.Implementation
{
    public class MemberNameFilterTest
    {
        readonly IFilter<Member<MemberInfo>> sut;

        // Constructor parameters
        readonly IFilter<Member<MemberInfo>> previous = Substitute.For<IFilter<Member<MemberInfo>>>();
        readonly string memberName = Guid.NewGuid().ToString();

        public MemberNameFilterTest() =>
            sut = new MemberNameFilter<Member<MemberInfo>, MemberInfo>(previous, memberName);

        public class Constructor: MemberNameFilterTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenPreviousFilterIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MemberNameFilter<Member<MemberInfo>, MemberInfo>(null, memberName));
                Assert.Equal("previous", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenMemberNameIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new MemberNameFilter<Member<MemberInfo>, MemberInfo>(previous, null));
                Assert.Equal("memberName", thrown.ParamName);
            }
        }

        public class MemberName: MemberNameFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(memberName, ((MemberNameFilter<Member<MemberInfo>, MemberInfo>)sut).MemberName);
        }

        public class Previous: MemberNameFilterTest
        {
            [Fact]
            public void ImplementsIDecoratorAndReturnsFilterGivenToConstructor() {
                var decorator = (IDecorator<IFilter<Member<MemberInfo>>>)sut;
                Assert.Same(previous, decorator.Previous);
            }
        }

        public class Get: MemberNameFilterTest
        {
            [Fact]
            public void ReturnsMembersWithGivenName() {
                Member<MemberInfo>[] expected = new[] { Member(memberName), Member(memberName) };
                Member<MemberInfo>[] mixed = new[] { Member(), expected[0], Member(), expected[1], Member() };
                previous.Get().Returns(mixed);

                IEnumerable<Member<MemberInfo>> actual = sut.Get();

                Assert.Equal(expected, actual);
            }

            class TestMember: Member<MemberInfo>
            {
                public TestMember(MemberInfo info) : base(info, null) { }
                public override bool IsStatic => true;
            }

            static Member<MemberInfo> Member(string name = default) {
                MemberInfo info = Substitute.For<MemberInfo>();
                info.Name.Returns(name ?? Guid.NewGuid().ToString());
                return new TestMember(info);
            }
        }
    }
}
