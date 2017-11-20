using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    [Collection(nameof(TypeInspector))]
    public class TypeInspectorFixture : IDisposable
    {
        readonly TypeInspector.Factory originalTypeInspectorCreate;
        protected readonly TypeInspector.Factory typeInspectorCreate = Substitute.For<TypeInspector.Factory>();
        protected readonly TypeInspector typeInspector = Substitute.For<TypeInspector>();

        public TypeInspectorFixture()
        {
            typeInspectorCreate.Invoke(Arg.Any<object>(), Arg.Any<Type>()).Returns(typeInspector);
            originalTypeInspectorCreate = ReplaceTypeInspectorCreate(typeInspectorCreate);
        }

        void IDisposable.Dispose()
        {
            ReplaceTypeInspectorCreate(originalTypeInspectorCreate);
        }

        static TypeInspector.Factory ReplaceTypeInspectorCreate(TypeInspector.Factory replacement)
        {
            FieldInfo create = typeof(TypeInspector).GetField(nameof(TypeInspector.Create));
            var current = (TypeInspector.Factory)create.GetValue(null);
            create.SetValue(null, replacement);
            return current;
        }
    }
}
