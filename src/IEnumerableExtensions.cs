using System.Collections.Generic;
using Inspector.Implementation;

namespace Inspector
{
    static class IEnumerableExtensions
    {
        public static T Single<T>(this IEnumerable<T> filter) =>
            Selector<T>.Select(filter);
    }
}
