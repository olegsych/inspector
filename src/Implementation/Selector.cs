using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    static class Selector<T>
    {
        internal static readonly Func<IEnumerable<T>, T> Select = (filter) => {
            if(filter == null)
                throw new ArgumentNullException(nameof(filter));

            return Enumerable.Single(filter);
        };
    }
}
