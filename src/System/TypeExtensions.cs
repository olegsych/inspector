using System.Collections.Generic;
using System.Reflection;
using Inspector;

namespace System
{
    public static class TypeExtensions
    {
        public static ConstructorInfo Constructor<T>(this Type type)
        {
            var inspector = TypeInspector.Create(type);
            return inspector.GetConstructor(typeof(T));
        }

        public static ConstructorInfo Constructor<T1, T2>(this Type type)
        {
            var inspector = TypeInspector.Create(type);
            return inspector.GetConstructor(typeof(T1), typeof(T2));
        }

        public static ConstructorInfo Constructor(this Type type, params Type[] parameters)
        {
            var inspector = TypeInspector.Create(type);
            return inspector.GetConstructor(parameters);
        }

        public static IReadOnlyList<ConstructorInfo> Constructors(this Type type)
        {
            var inspector = TypeInspector.Create(type);
            return inspector.GetConstructors();
        }
    }
}
