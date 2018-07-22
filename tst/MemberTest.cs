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
            sut = new InstanceMember(info, instance);

        public class InstanceMember : Member<MemberInfo>
        {
            public InstanceMember(MemberInfo info, object instance) : base(info, instance) { }
            public override bool IsStatic => false;
        }

        public class StaticMember : Member<MemberInfo>
        {
            public StaticMember(MemberInfo info, object instance) : base(info, instance) { }
            public override bool IsStatic => true;
        }

        public class Constructor : MemberTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenMemberInfoIsNullToFailFast() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new InstanceMember(null, instance));
                Assert.Equal("info", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionGivenInstanceMemberInfoWithoutInstance() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new InstanceMember(info, null));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance is required for {info}", thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionGivenInstanceForStaticMember() {
                MemberInfo staticMember = typeof(InstanceType).GetField(nameof(InstanceType.StaticField));
                var thrown = Assert.Throws<ArgumentException>(() => new StaticMember(staticMember, instance));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance shouldn't be specified for static {staticMember}.", thrown.Message);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenGivenInstanceOfTypeDifferentFromMemberInfo() {
                var thrown = Assert.Throws<ArgumentException>(() => new InstanceMember(info, new AnotherType()));
                Assert.Equal("instance", thrown.ParamName);
                Assert.StartsWith($"Instance type {nameof(AnotherType)} doesn't match type {info.DeclaringType.Name} where {info.Name} is declared.", thrown.Message);
            }

            [Fact]
            public void InitializesNewInstanceForDerivedInstanceMember() {
                var derived = new DerivedType();

                var sut = new InstanceMember(info, derived);

                Assert.Same(info, sut.Info);
                Assert.Same(derived, sut.Instance);
            }

            [Fact]
            public void AcceptsNullInstanceToSupportStaticMembers() =>
                new StaticMember(info, null);
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
            public static object StaticField = new object();
        }

        class DerivedType : InstanceType { }

        class AnotherType { }
    }
}
