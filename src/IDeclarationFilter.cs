using System;

namespace Inspector
{
    /// <summary>
    /// Represents a pattern that <see cref="ObjectInspector"/> and <see cref="TypeInspector"/> should implement.
    /// This is a design concept only. Remove, unless it's actually used.
    /// </summary>
    interface IDeclarationFilter<T> where T : IDeclarationFilter<T>
    {
        T Declared(Type declaringType = default);

        T Inherited(Type baseType = default);
    }
}
