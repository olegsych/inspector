using System;

namespace Inspector
{
    /// <summary>
    /// Creates instances of type <typeparamref name="T"/> dynamically.
    /// </summary>
    public static class Type<T>
    {
        /// <summary>
        /// Creates an instance of type <typeparamref name="T"/> using the constructor that best matches given <paramref name="args"/>.
        /// </summary>
        public static T New(params object[] args) =>
            (T)typeof(T).New(args);

        /// <summary>
        /// Creates an uninitialized instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// A zeroed object of type <typeparamref name="T"/>, created without invoking any instance constructors.
        /// </returns>
        public static T Uninitialized() =>
            (T)typeof(T).Uninitialized();
    }
}
