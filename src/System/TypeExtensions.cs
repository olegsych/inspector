using System.Collections.Generic;
using System.Reflection;
using Inspector;

namespace System
{
    public static class TypeExtensions
    {
        public static ConstructorInfo Constructor(this Type type)
        {
            var inspector = TypeInspector.Create(type);
            return inspector.GetConstructor();
        }

        public static IReadOnlyList<ConstructorInfo> Constructors(this Type type)
        {
            var inspector = TypeInspector.Create(type);
            return inspector.GetConstructors();
        }
    }
}
