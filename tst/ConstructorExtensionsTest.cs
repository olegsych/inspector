using System;
using NSubstitute;
using Xunit;

namespace Inspector
{
    [Collection(nameof(ConstructorExtensionsTest))]
    public class ConstructorExtensionsTest : SelectorFixture<Constructor>
    {
        // Shared test fixture
        class TestType { }
        readonly object instance = new TestType();
        readonly Constructor selected;
        IFilter<Constructor> selection;

        public ConstructorExtensionsTest() {
            selected = new Constructor(typeof(TestType).GetConstructor(new Type[0]), instance);
            select.Invoke(Arg.Do<IFilter<Constructor>>(_ => selection = _)).Returns(selected);
        }

        public class IScopeExtension : ConstructorExtensionsTest
        {
            // Method parameters
            readonly IScope scope = Substitute.For<IScope>();

            [Fact]
            public void ReturnsSingleConstructorInGivenScope() {
                Assert.Same(selected, scope.Constructor());
                Assert.Same(scope, selection);
            }
        }

        public class ObjectExtension : ConstructorExtensionsTest
        {
            [Fact]
            public void ReturnsSingleConstructorOfGivenInstance() {
                Assert.Same(selected, instance.Constructor());
                VerifyScope(selection, instance);
            }

            static void VerifyScope(IFilter<Constructor> filter, object instance) {
                var scope = Assert.IsType<InstanceScope>(filter);
                Assert.Same(instance, scope.Instance);
            }
        }

        public class TypeExtension : ConstructorExtensionsTest
        {
            // Method parameters
            readonly Type type;

            public TypeExtension() =>
                type = instance.GetType();

            [Fact]
            public void ReturnsSingleConstructorOfGivenType() {
                Assert.Same(selected, type.Constructor());
                VerifyScope(selection, type);
            }

            static void VerifyScope(IFilter<Constructor> filter, Type type) {
                var scope = Assert.IsType<StaticScope>(filter);
                Assert.Same(type, scope.Type);
            }
        }
    }
}
