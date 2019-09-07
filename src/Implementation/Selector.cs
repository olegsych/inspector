using System;
using System.Linq;

namespace Inspector.Implementation
{
    static class Selector<T>
    {
        internal static readonly Func<IFilter<T>, T> Select = (filter) => {
            if(filter == null)
                throw new ArgumentNullException(nameof(filter));

            return filter.Get().Single();
        };
    }
}
