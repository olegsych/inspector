using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;
using static Inspector.Substitutes;

namespace Inspector.Implementation
{
    public class DeclaredMembersTest
    {
        readonly Filter<Member<MemberInfo>> sut;

        // Constructor parameters
        readonly IEnumerable<Member<MemberInfo>> previous = Substitute.For<IEnumerable<Member<MemberInfo>>>();
        readonly Type declaringType = typeof(ExpectedType);

        public DeclaredMembersTest() =>
            sut = new DeclaredMembers<Member<MemberInfo>, MemberInfo>(previous, declaringType);

        public class Constructor: DeclaredMembersTest
        {
            [Fact]
            public void PassesPreviousToBase() =>
                Assert.Same(previous, sut.Previous);

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDeclaringTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new DeclaredMembers<Member<MemberInfo>, MemberInfo>(previous, null));
                Assert.Equal("declaringType", thrown.ParamName);
            }
        }

        public class DeclaringType: DeclaredMembersTest
        {
            [Fact]
            public void IsInitializedByConstructor() =>
                Assert.Equal(declaringType, ((DeclaredMembers<Member<MemberInfo>, MemberInfo>)sut).DeclaringType);
        }

        public class GetEnumerator: DeclaredMembersTest
        {
            [Fact]
            public void ReturnsMembersWithMatchingDeclaringType() {
                Member<MemberInfo>[] expected = new[] { Member(declaringType), Member(declaringType) };
                IEnumerable<Member<MemberInfo>> mixed = new[] { Member(), expected[0], Member(), expected[1], Member() };
                ConfiguredCall arrange = previous.GetEnumerator().Returns(mixed.GetEnumerator());
                Assert.Equal(expected, sut);
            }

            class TestMember: Member<MemberInfo>
            {
                public TestMember(MemberInfo info) : base(info, null) { }
                public override bool IsStatic => true;
            }

            static Member<MemberInfo> Member(Type declaringType = default) {
                MemberInfo info = Substitute.For<MemberInfo>();
                ConfiguredCall arrange = info.DeclaringType.Returns(declaringType ?? typeof(UnexpectedType));
                return new TestMember(info);
            }
        }

        class ExpectedType { }
        class UnexpectedType { }
    }
}
