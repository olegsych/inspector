using System.Collections.Generic;
using System.Reflection;
using Inspector;

namespace System
{
    public static class ObjectExtensions
    {
        public static ConstructorInfo Constructor<T>(this T instance, params Type[] parameters)
        {
            var inspector = TypeInspector.Create(typeof(T), instance);
            return inspector.GetConstructor(parameters);
        }

        public static IReadOnlyList<ConstructorInfo> Constructors<T>(this T instance)
        {
            var inspector = TypeInspector.Create(typeof(T), instance);
            return inspector.GetConstructors();
        }
    }
}
