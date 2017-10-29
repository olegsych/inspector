using System.Linq;
using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        public static ConstructorInfo Constructor<T>(this T instance)
        {
            TypeInfo typeInfo = instance?.GetType().GetTypeInfo() ?? typeof(T).GetTypeInfo();
            return typeInfo.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single();
        }
    }
}
