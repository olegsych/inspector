using Inspector.Implementation;

namespace Inspector
{
    static class IFilterExtensions
    {
        public static T Single<T>(this IFilter<T> filter) =>
            Selector<T>.Select(filter);
    }
}
