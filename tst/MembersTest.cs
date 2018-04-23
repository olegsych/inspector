using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class MembersTest
    {
        // Constructor parameters
        readonly Type type = typeof(TestType);
        readonly object instance = new TestType();
        readonly Members<MemberInfo, Member<MemberInfo>>.InfoProvider infoProvider = Substitute.For<Members<MemberInfo, Member<MemberInfo>>.InfoProvider>();
        readonly Members<MemberInfo, Member<MemberInfo>>.Factory createMember = Substitute.For<Members<MemberInfo, Member<MemberInfo>>.Factory>();

        // Test fixture
        readonly Func<BindingFlags, IEnumerable<MemberInfo>> getMemberInfo = Substitute.For<Func<BindingFlags, IEnumerable<MemberInfo>>>();

        public class Constructor : MembersTest
        {
            [Fact]
            public void ThrowsDescriptiveExceptionWhenTypeIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(null, instance, infoProvider, createMember));
                Assert.Equal("type", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenInfoProviderIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(type, instance, null, createMember));
                Assert.Equal("getMemberInfo", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenFactoryIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Members<MemberInfo, Member<MemberInfo>>(type, instance, infoProvider, null));
                Assert.Equal("createMember", thrown.ParamName);
            }
        }

        public abstract class GetEnumeratorTest : MembersTest
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
                createMember.Invoke(Arg.Any<MemberInfo>(), Arg.Any<object>()).Returns(_ => Substitute.ForPartsOf<Member<MemberInfo>>(_.Arg<MemberInfo>(), _.Arg<object>()));

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

        public class InstanceEnumerator : GetEnumeratorTest
        {
            public InstanceEnumerator() {
                sut = new Members<MemberInfo, Member<MemberInfo>>(type, instance, infoProvider, createMember);
                getEnumerator = sut.GetEnumerator;
                expectedBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance;
                expectedInstance = instance;
            }
        }

        public class StaticEnumerator : GetEnumeratorTest
        {
            public StaticEnumerator() {
                sut = new Members<MemberInfo, Member<MemberInfo>>(type, null, infoProvider, createMember);
                getEnumerator = sut.GetEnumerator;
                expectedBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static;
                expectedInstance = null;
            }
        }

        public class UntypedInstanceEnumerator : InstanceEnumerator
        {
            public UntypedInstanceEnumerator() {
                getEnumerator = ((IEnumerable)sut).GetEnumerator;
            }
        }

        public class UntypedStaticEnumerator : InstanceEnumerator
        {
            public UntypedStaticEnumerator() {
                getEnumerator = ((IEnumerable)sut).GetEnumerator;
            }
        }

        class BaseType
        {
            public void BaseMethod1() { }
            public void BaseMethod2() { }
        }

        class TestType : BaseType
        {
            public void TestMethod1() { }
            public void TestMethod2() { }
        }
    }
}
