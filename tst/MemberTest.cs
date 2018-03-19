using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class MemberTest
    {
        readonly Member<MemberInfo> sut;

        // Constructor parameters
        readonly MemberInfo info = typeof(InstanceType).GetField(nameof(InstanceType.Field));
        readonly object instance = new InstanceType();

        public MemberTest() =>
            sut = Substitute.ForPartsOf<Member<MemberInfo>>(info, instance);

        public class Constructor : MemberTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenMemberInfoIsNullToFailFast() {
                var thrown = Assert.Throws<TargetInvocationException>(() => Substitute.ForPartsOf<Member<MemberInfo>>(null, instance));
                var actual = Assert.IsType<ArgumentNullException>(thrown.InnerException);
                Assert.Equal("info", actual.ParamName);
            }

            [Fact]
            public void AcceptsNullInstanceToSupportStaticMembers() =>
                Substitute.ForPartsOf<Member<MemberInfo>>(info, null);
        }

        public class Info : MemberTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(info, sut.Info);
        }

        public class Instance : MemberTest
        {
            [Fact]
            public void ReturnsValueGivenToConstructor() =>
                Assert.Same(instance, sut.Instance);
        }

        public class TMemberInfo : MemberTest
        {
            [Fact]
            public void ImplicitlyConvertsMemberToMemberInfo() {
                MemberInfo converted = sut;
                Assert.Same(info, converted);
            }

            [Fact]
            public void ConvertsNullToNullWithoutThrowingExceptionToSupportImplicitConversionRules() {
                MemberInfo converted = ((Member<MemberInfo>)null);
                Assert.Null(converted);
            }
        }

        class InstanceType
        {
#pragma warning disable 649 // Field accessed only via reflection
            public object Field;
#pragma warning restore 649
        }
    }
}
