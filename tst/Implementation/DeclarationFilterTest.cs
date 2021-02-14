using System;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace Inspector.Implementation
{
    public class DeclarationFilterTest
    {
        readonly Filter<Member<MemberInfo>> sut;

        // Constructor parameters
        readonly IEnumerable<Member<MemberInfo>> source = Substitute.For<IEnumerable<Member<MemberInfo>>>();
        readonly Type declaringType = typeof(ExpectedType);

        public DeclarationFilterTest() =>
            sut = new DeclarationFilter<Member<MemberInfo>, MemberInfo>(source, declaringType);

        public class Constructor: DeclarationFilterTest
        {
            [Fact]
            public void PassesSourceToBase() =>
                Assert.Same(source, sut.Source);

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDeclaringTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new DeclarationFilter<Member<MemberInfo>, MemberInfo>(source, null!));
                Assert.Equal("declaringType", thrown.ParamName);
            }
        }

        public class DeclaringType: DeclarationFilterTest
        {
            [Fact]
            public void IsInitializedByConstructor() =>
                Assert.Equal(declaringType, ((DeclarationFilter<Member<MemberInfo>, MemberInfo>)sut).DeclaringType);
        }

        public class GetEnumerator: DeclarationFilterTest
        {
            [Fact]
            public void ReturnsMembersWithMatchingDeclaringType() {
                Member<MemberInfo>[] expected = new[] { Member(declaringType), Member(declaringType) };
                IEnumerable<Member<MemberInfo>> mixed = new[] { Member(), expected[0], Member(), expected[1], Member() };
                ConfiguredCall arrange = source.GetEnumerator().Returns(mixed.GetEnumerator());
                Assert.Equal(expected, sut);
            }

            class TestMember: Member<MemberInfo>
            {
                public TestMember(MemberInfo info) : base(info, null) { }
                public override bool IsStatic => true;
            }

            static Member<MemberInfo> Member(Type? declaringType = default) {
                MemberInfo info = Substitute.For<MemberInfo>();
                ConfiguredCall arrange = info.DeclaringType.Returns(declaringType ?? typeof(UnexpectedType));
                return new TestMember(info);
            }
        }

        class ExpectedType { }
        class UnexpectedType { }
    }
}
