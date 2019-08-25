using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector.Implementation
{
    public class MembersTest
    {
        // Constructor parameters
        readonly Type type = typeof(TestType);
        readonly object instance = new TestType();
        readonly Members<MemberInfo, Member<MemberInfo>>.InfoProvider infoProvider = Substitute.For<Members<MemberInfo, Member<MemberInfo>>.InfoProvider>();
        readonly Members<MemberInfo, Member<MemberInfo>>.Factory createMember = Substitute.For<Members<MemberInfo, Member<MemberInfo>>.Factory>();
        readonly Lifetime lifetime = Lifetime.Instance;

        // Test fixture
        readonly Func<BindingFlags, IEnumerable<MemberInfo>> getMemberInfo = Substitute.For<Func<BindingFlags, IEnumerable<MemberInfo>>>();

        public class Constructor: MembersTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(null, instance, infoProvider, createMember, lifetime));
                Assert.Equal("type", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInfoProviderIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(type, instance, null, createMember, lifetime));
                Assert.Equal("getMemberInfo", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFactoryIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(type, instance, infoProvider, null, lifetime));
                Assert.Equal("createMember", thrown.ParamName);
            }

            [Fact]
            public void InitializesInstanceWithGivenArguments() {
                var sut = new Members<MemberInfo, Member<MemberInfo>>(type, instance, infoProvider, createMember, lifetime);

                Assert.Same(type, sut.Type);
                Assert.Same(instance, sut.Instance);
                Assert.Same(infoProvider, sut.GetMemberInfo);
                Assert.Same(createMember, sut.CreateMember);
            }

            [Fact]
            public void InitializesDefaultLifetimeToInstanceWhenInstanceIsSpecified() {
                var sut = new Members<MemberInfo, Member<MemberInfo>>(type, instance, infoProvider, createMember);
                Assert.Equal(Lifetime.Instance, sut.Lifetime);
            }

            [Fact]
            public void InitializesDefaultLifetimeStaticWhenInstanceIsNull() {
                var sut = new Members<MemberInfo, Member<MemberInfo>>(type, null, infoProvider, createMember);
                Assert.Equal(Lifetime.Static, sut.Lifetime);
            }

            [Fact]
            public void OverridesLifetimeToInstanceWhenInstanceIsNull() {
                var sut = new Members<MemberInfo, Member<MemberInfo>>(type, null, infoProvider, createMember, Lifetime.Instance);
                Assert.Equal(Lifetime.Instance, sut.Lifetime);
            }

            [Fact]
            public void OverridesLifetimeToStaticWhenInstanceIsSpecified() {
                var sut = new Members<MemberInfo, Member<MemberInfo>>(type, instance, infoProvider, createMember, Lifetime.Static);
                Assert.Equal(Lifetime.Static, sut.Lifetime);
            }
        }

        public abstract class GetEnumeratorTest: MembersTest
        {
            protected IEnumerable<Member<MemberInfo>> sut;
            protected Func<IEnumerator> getEnumerator;
            protected BindingFlags expectedBinding;
            protected object expectedInstance;

            [Fact]
            public void GetEnumeratorReturnsMembersOfAllTypesInHierarchy() {
                // Arrange
                MemberInfo[] testTypeMembers = { typeof(TestType).GetMethod(nameof(TestType.TestMethod1)), typeof(TestType).GetMethod(nameof(TestType.TestMethod2)) };
                MemberInfo[] baseTypeMembers = { typeof(BaseType).GetMethod(nameof(BaseType.BaseMethod1)), typeof(BaseType).GetMethod(nameof(BaseType.BaseMethod2)) };
                MemberInfo[] rootTypeMembers = { typeof(object).GetMethod(nameof(object.GetType)), typeof(object).GetMethod(nameof(object.GetHashCode)) };

                infoProvider.Invoke(typeof(TestType).GetTypeInfo()).Returns(getMemberInfo);
                infoProvider.Invoke(typeof(BaseType).GetTypeInfo()).Returns(getMemberInfo);
                infoProvider.Invoke(typeof(object).GetTypeInfo()).Returns(getMemberInfo);

                getMemberInfo.Invoke(expectedBinding).Returns(testTypeMembers, baseTypeMembers, rootTypeMembers);

                // Act
                IEnumerator enumerator = getEnumerator();

                // Assert
                foreach(MemberInfo memberInfo in testTypeMembers) {
                    Assert.True(enumerator.MoveNext());
                    var current = Assert.IsAssignableFrom<Member<MemberInfo>>(enumerator.Current);
                    Assert.Same(memberInfo, current.Info);
                    Assert.Same(expectedInstance, current.Instance);
                }

                foreach(MemberInfo memberInfo in baseTypeMembers) {
                    Assert.True(enumerator.MoveNext());
                    var current = Assert.IsAssignableFrom<Member<MemberInfo>>(enumerator.Current);
                    Assert.Same(memberInfo, current.Info);
                    Assert.Same(expectedInstance, current.Instance);
                }

                foreach(MemberInfo memberInfo in rootTypeMembers) {
                    Assert.True(enumerator.MoveNext());
                    var current = Assert.IsAssignableFrom<Member<MemberInfo>>(enumerator.Current);
                    Assert.Same(memberInfo, current.Info);
                    Assert.Same(expectedInstance, current.Instance);
                }

                Assert.False(enumerator.MoveNext());
            }
        }

        public class InstanceEnumerator: GetEnumeratorTest
        {
            public InstanceEnumerator() {
                sut = new Members<MemberInfo, Member<MemberInfo>>(type, instance, infoProvider, createMember);
                getEnumerator = sut.GetEnumerator;
                expectedBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance;
                expectedInstance = instance;
                createMember.Invoke(Arg.Any<MemberInfo>(), Arg.Any<object>()).Returns(_ => new InstanceMember(_.Arg<MemberInfo>(), _.Arg<object>()));
            }
        }

        public abstract class TypeEnumerator: GetEnumeratorTest
        {
            private protected TypeEnumerator(Lifetime lifetime) {
                sut = new Members<MemberInfo, Member<MemberInfo>>(type, null, infoProvider, createMember, lifetime);
                getEnumerator = sut.GetEnumerator;
                expectedBinding = (BindingFlags)lifetime | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
                expectedInstance = null;
                createMember.Invoke(Arg.Any<MemberInfo>(), Arg.Any<object>()).Returns(_ => new StaticMember(_.Arg<MemberInfo>(), _.Arg<object>()));
            }
        }

        public class TypeEnumeratorWithStaticBinding: TypeEnumerator
        {
            public TypeEnumeratorWithStaticBinding() : base(Lifetime.Static) { }
        }

        public class TypeEnumeratorWithInstanceBinding: TypeEnumerator
        {
            public TypeEnumeratorWithInstanceBinding() : base(Lifetime.Instance) { }
        }

        public class UntypedInstanceEnumerator: InstanceEnumerator
        {
            public UntypedInstanceEnumerator() => getEnumerator = ((IEnumerable)sut).GetEnumerator;
        }

        public class UntypedTypeEnumeratorWithStaticBinding: TypeEnumeratorWithStaticBinding
        {
            public UntypedTypeEnumeratorWithStaticBinding() => getEnumerator = ((IEnumerable)sut).GetEnumerator;
        }

        public class UntypedTypeEnumeratorWithInstanceBinding: TypeEnumeratorWithInstanceBinding
        {
            public UntypedTypeEnumeratorWithInstanceBinding() => getEnumerator = ((IEnumerable)sut).GetEnumerator;
        }

        class BaseType
        {
            public void BaseMethod1() { }
            public void BaseMethod2() { }
        }

        class TestType: BaseType
        {
            public void TestMethod1() { }
            public void TestMethod2() { }
        }

        class InstanceMember: Member<MemberInfo>
        {
            public InstanceMember(MemberInfo info, object instance) : base(info, instance) { }
            public override bool IsStatic => false;
        }

        class StaticMember: Member<MemberInfo>
        {
            public StaticMember(MemberInfo info, object instance) : base(info, instance) { }
            public override bool IsStatic => true;
        }
    }
}
