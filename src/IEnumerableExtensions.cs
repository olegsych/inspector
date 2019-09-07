using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    static class IEnumerableExtensions
    {
        public static T Single<T>(this IEnumerable<T> filter) =>
            Selector<T>.Select(filter);
    }
}
