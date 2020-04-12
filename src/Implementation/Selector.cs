using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    static class Selector<T>
    {
        internal static
#if !DEBUG
        readonly // Keep writable for unit tests. See https://github.com/dotnet/runtime/issues/11571
#endif
        Func<IEnumerable<T>, T> Select = (filter) => {
            if(filter == null)
                throw new ArgumentNullException(nameof(filter));

            return Enumerable.Single(filter);
        };
    }
}
