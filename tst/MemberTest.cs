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
            public void ThrowsDescriptiveExceptionWhenGivenInstanceOfTypeDifferentFromMemberInfo() {
                var thrown = Assert.Throws<TargetInvocationException>(() => Substitute.ForPartsOf<Member<MemberInfo>>(info, new AnotherType()));
                var actual = Assert.IsType<ArgumentException>(thrown.InnerException);
                Assert.Equal("instance", actual.ParamName);
                Assert.StartsWith($"Instance type {nameof(AnotherType)} doesn't match type {info.DeclaringType.Name} where {info.Name} is declared.", actual.Message);
            }

            [Fact]
            public void InitializesNewInstanceForDerivedInstanceMember() {
                var derived = new DerivedType();

                var sut = Substitute.ForPartsOf<Member<MemberInfo>>(info, derived);

                Assert.Same(info, sut.Info);
                Assert.Same(derived, sut.Instance);
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
            public object Field = new object();
        }

        class DerivedType : InstanceType { }

        class AnotherType { }
    }
}
