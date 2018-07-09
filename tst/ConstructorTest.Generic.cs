using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class GenericConstructorTest
    {
        readonly Constructor<TestSignature> sut;

        // Constructor parameters
        readonly Constructor constructor;
        readonly IDelegateFactory<ConstructorInfo> delegateFactory = Substitute.For<IDelegateFactory<ConstructorInfo>>();

        // Test fixture
        readonly TestType instance = (TestType)FormatterServices.GetUninitializedObject(typeof(TestType));
        readonly TestSignature @delegate = Substitute.For<TestSignature>();

        public GenericConstructorTest() {
            ConstructorInfo info = typeof(TestType).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single();
            constructor = new Constructor(info, instance);

            delegateFactory.TryCreate(Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<ConstructorInfo>(), out Delegate _)
                .Returns(args => {
                    args[3] = @delegate;
                    return true;
                });

            sut = new Constructor<TestSignature>(constructor, delegateFactory);
        }

        public class ConstructorTest : GenericConstructorTest
        {
            [Fact]
            public void InitializesBaseWithGivenArgument() {
                Constructor @base = sut;
                Assert.Same(constructor.Info, @base.Info);
                Assert.Same(constructor.Instance, @base.Instance);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenConstructorIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Constructor<TestSignature>(null, delegateFactory));
                Assert.Equal("constructor", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateFactoryIsNull() {
                var thrown = Assert.Throws<ArgumentNullException>(() => new Constructor<TestSignature>(constructor, null));
                Assert.Equal("delegateFactory", thrown.ParamName);
            }

            [Fact]
            public void ThrowsDescriptiveExceptionWhenDelegateFactoryCannotCreateDelegateForGivenConstructor() {
                delegateFactory.TryCreate(Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<ConstructorInfo>(), out Delegate _).Returns(false);

                var thrown = Assert.Throws<ArgumentException>(() => new Constructor<TestSignature>(constructor, delegateFactory));
                Assert.Equal("constructor", thrown.ParamName);
                Assert.StartsWith($"Constructor {constructor.Info} doesn't match expected signature.", thrown.Message);
            }
        }

        public class Invoke: GenericConstructorTest
        {
            [Fact]
            public void ReturnsDelegateCreatedByDelegateFactoryForGivenConstructor() {
                Assert.Same(@delegate, sut.Invoke);
                delegateFactory.Received().TryCreate(typeof(TestSignature), sut.Instance, sut.Info, out Delegate _);
                delegateFactory.Received(1).TryCreate(Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<ConstructorInfo>(), out Delegate _);
            }
        }

        public class ImplicitOperatorT : GenericConstructorTest
        {
            [Fact]
            public void ImplicitlyConvertsConstructorToItsSignatureDelegate() {
                TestSignature actual = sut;
                Assert.Same(sut.Invoke, actual);
            }

            [Fact]
            public void ConvertsNullConstructorToNullSignatureToSupportImplicitConversionRules() {
                TestSignature actual = ((Constructor<TestSignature>)null);
                Assert.Null(actual);
            }
        }

        internal class TestType
        {
            TestType(P1 p1, P2 p2) { }
        }

        internal class P1 { }

        internal class P2 { }

        delegate void TestSignature(P1 p1, P2 p2);
    }
}
