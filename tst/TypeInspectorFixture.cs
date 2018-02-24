using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector {

    [Collection(nameof(TypeInspector))]
    public class TypeInspectorFixture : IDisposable {

        readonly TypeInspector.Factory originalTypeInspectorCreate;
        internal readonly TypeInspector.Factory typeInspectorCreate = Substitute.For<TypeInspector.Factory>();
        internal readonly TypeInspector typeInspector = Substitute.For<TypeInspector>();

        public TypeInspectorFixture() {
            typeInspectorCreate.Invoke(Arg.Any<Type>(), Arg.Any<object>()).Returns(typeInspector);
            originalTypeInspectorCreate = ReplaceTypeInspectorCreate(typeInspectorCreate);
        }

        void IDisposable.Dispose()
            => ReplaceTypeInspectorCreate(originalTypeInspectorCreate);

        static TypeInspector.Factory ReplaceTypeInspectorCreate(TypeInspector.Factory replacement) {
            FieldInfo create = typeof(TypeInspector).GetField(nameof(TypeInspector.Create));
            var current = (TypeInspector.Factory)create.GetValue(null);
            create.SetValue(null, replacement);
            return current;
        }
    }
}
