using System;

namespace Inspector
{
    class Selector<T>
    {
        internal static readonly Func<IFilter<T>, T> Select = (filter) => default;
    }
}
