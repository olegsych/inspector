using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Inspector
{
    /// <summary>
    /// <see cref="Type"/> extension methods for creating instances dynamically.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Creates an instance of given <see cref="Type"/> using the constructor that best matches given <paramref name="args"/>.
        /// </summary>
        public static object New(this Type type, params object[] args) =>
            Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, default, args, default);

        /// <summary>
        /// Creates an uninitialized instance of given <see cref="Type"/>.
        /// </summary>
        /// <returns>
        /// A zeroed object of given type, created without invoking any instance constructors.
        /// </returns>
        public static object Uninitialized(this Type type) =>
            FormatterServices.GetUninitializedObject(type);
    }
}
