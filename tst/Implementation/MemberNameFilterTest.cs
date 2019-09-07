using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Inspector.Implementation
{
    public class MemberNameFilterTest
    {
        readonly Filter<Member<MemberInfo>> sut;

        // Constructor parameters
        readonly IEnumerable<Member<MemberInfo>> previous = Substitute.For<IEnumerable<Member<MemberInfo>>>();
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

            [Fact]
            public void PassesPreviousToBaseConstructor() =>
                Assert.Same(previous, sut.Previous);
        }

        public class MemberName: MemberNameFilterTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(memberName, ((MemberNameFilter<Member<MemberInfo>, MemberInfo>)sut).MemberName);
        }

        public class GetEnumerator: MemberNameFilterTest
        {
            [Fact]
            public void ReturnsMembersWithGivenName() {
                Member<MemberInfo>[] expected = new[] { Member(memberName), Member(memberName) };
                IEnumerable<Member<MemberInfo>> mixed = new[] { Member(), expected[0], Member(), expected[1], Member() };
                ConfiguredCall arrange = previous.GetEnumerator().Returns(mixed.GetEnumerator());

                Assert.Equal(expected, sut);
            }

            class TestMember: Member<MemberInfo>
            {
                public TestMember(MemberInfo info) : base(info, null) { }
                public override bool IsStatic => true;
            }

            static Member<MemberInfo> Member(string name = default) {
                MemberInfo info = Substitute.For<MemberInfo>();
                ConfiguredCall arrange = info.Name.Returns(name ?? Guid.NewGuid().ToString());
                return new TestMember(info);
            }
        }
    }
}
