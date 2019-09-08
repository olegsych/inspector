using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class IMembersExtensionsTest
    {
        readonly IMembers members = Substitute.For<IMembers>();

        public class ConstructorMethod: ConstructorExtensionsTest
        {
            readonly IMembers members = Substitute.For<IMembers>();

            // Test fixture
            readonly IEnumerable<Constructor> constructors = Substitute.For<IEnumerable<Constructor>>();

            public ConstructorMethod() =>
                members.Constructors().Returns(constructors);

            [Fact]
            public void ReturnsSingleConstructor() {
                Constructor actual = members.Constructor();

                Assert.Same(selected, actual);
                Assert.Same(constructors, selection);
            }

            [Fact]
            public void ReturnsConstructorWithGivenDelegateType() {
                Constructor actual = members.Constructor(delegateType);

                Assert.Same(selected, actual);
                ConstructorTypeFilter filter = VerifyFilter(selection, delegateType);
                Assert.Same(constructors, filter.Source);
            }

            [Fact]
            public void ReturnsGenericConstructorWithGivenSignature() {
                Constructor<TestDelegate> generic = members.Constructor<TestDelegate>();

                VerifyGenericConstructor(selected, generic);
                ConstructorTypeFilter typeFilter = VerifyFilter(selection, typeof(TestDelegate));
                Assert.Same(constructors, typeFilter.Source);
            }
        }

        [Fact]
        public void DeclaredByReturnsMembersDeclaredByGivenType() {
            IMembers actual = members.DeclaredBy(typeof(TestType));

            VerifyDeclaredMembers<TestType>(members, actual);
        }

        [Fact]
        public void DeclaredByGenericReturnsMembersDeclaredByGivenType() {
            IMembers actual = members.DeclaredBy<TestType>();

            VerifyDeclaredMembers<TestType>(members, actual);
        }

        public class EventMethod: EventExtensionsTest
        {
            readonly IMembers members = Substitute.For<IMembers>();

            // Test fixture
            readonly IEnumerable<Event> events = Substitute.For<IEnumerable<Event>>();

            public EventMethod() =>
                members.Events().Returns(events);

            [Fact]
            public void ReturnsSingleEvent() {
                Assert.Same(selected, members.Event());
                Assert.Same(events, selection);
            }

            [Fact]
            public void ReturnsEventWithGivenName() {
                Assert.Same(selected, members.Event(eventName));
                MemberNameFilter<Event, EventInfo> filter = VerifyFilter(selection, eventName);
                Assert.Same(events, filter.Source);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerType() {
                Assert.Same(selected, members.Event(handlerType));
                EventTypeFilter filter = VerifyFilter(selection, handlerType);
                Assert.Same(events, filter.Source);
            }

            [Fact]
            public void ReturnsEventWithGivenHandlerTypeAndName() {
                Assert.Same(selected, members.Event(handlerType, eventName));
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                Assert.Same(events, typed.Source);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerType() {
                Event<TestHandler> generic = members.Event<TestHandler>();
                VerifyGenericEvent(selected, generic);
                EventTypeFilter typed = VerifyFilter(selection, handlerType);
                Assert.Same(events, typed.Source);
            }

            [Fact]
            public void ReturnsGenericEventWithGivenHandlerTypeAndName() {
                Event<TestHandler> generic = members.Event<TestHandler>(eventName);
                VerifyGenericEvent(selected, generic);
                MemberNameFilter<Event, EventInfo> named = VerifyFilter(selection, eventName);
                EventTypeFilter typed = VerifyFilter(named.Source, handlerType);
                Assert.Same(events, typed.Source);
            }
        }

        [Fact]
        public void InternalReturnsInternalMembers() {
            IMembers actual = members.Internal();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Internal, accessibleMembers.Accessibility);
        }

        [Fact]
        public void InheritedFromReturnsMembersInheritedFromGivenType() {
            IMembers actual = members.InheritedFrom(typeof(TestType));

            VerifyInheritedMembers<TestType>(members, actual);
        }

        [Fact]
        public void InheritedFromGenericReturnsMembersInheritedFromGivenType() {
            IMembers actual = members.InheritedFrom<TestType>();

            VerifyInheritedMembers<TestType>(members, actual);
        }

        [Fact]
        public void PrivateReturnsPrivateMembers() {
            IMembers actual = members.Private();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Private, accessibleMembers.Accessibility);
        }

        [Fact]
        public void ProtectedReturnsProtectedMembers() {
            IMembers actual = members.Protected();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Protected, accessibleMembers.Accessibility);
        }

        [Fact]
        public void PublicReturnsPublicMembers() {
            IMembers actual = members.Public();

            var accessibleMembers = Assert.IsType<AccessibleMembers>(actual);
            Assert.Equal(Accessibility.Public, accessibleMembers.Accessibility);
        }

        static void VerifyDeclaredMembers<TDeclaringType>(IMembers source, IMembers actual) {
            var declaredMembers = Assert.IsType<DeclaredMembers>(actual);
            Assert.Equal(typeof(TDeclaringType), declaredMembers.DeclaringType);
            Assert.Same(source, declaredMembers.Source);
        }

        static void VerifyInheritedMembers<TAncestorType>(IMembers source, IMembers actual) {
            var inheritedMembers = Assert.IsType<InheritedMembers>(actual);
            Assert.Equal(typeof(TAncestorType), inheritedMembers.AncestorType);
            Assert.Same(source, inheritedMembers.Source);
        }

        class TestType {}
    }
}
