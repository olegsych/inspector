using System;

namespace Inspector
{
    /// <summary>
    /// Represents a pattern that <see cref="ObjectExtensions"/> and <see cref="TypeExtensions"/> should implement.
    /// This is a design concept only. Remove, unless it's actually used.
    /// </summary>
    static class IDeclarationFilterExtensions
    {
        public static T Declared<T>(this T declarationFilter) where T : IDeclarationFilter<T> =>
            throw new NotImplementedException();

        public static T Inherited<T>(this T declarationFilter) where T : IDeclarationFilter<T> =>
            throw new NotImplementedException();
    }
}
