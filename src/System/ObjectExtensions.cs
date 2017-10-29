using System.Collections.Generic;
using System.Reflection;
using Inspector;

namespace System
{
    public static class ObjectExtensions
    {
        public static ConstructorInfo Constructor<T>(this T instance)
        {
            var inspector = TypeInspector.Create(instance, typeof(T));
            return inspector.GetConstructor();
        }

        public static IReadOnlyList<ConstructorInfo> Constructors<T>(this T instance)
        {
            var inspector = TypeInspector.Create(instance, typeof(T));
            return inspector.GetConstructors();
        }
    }
}
