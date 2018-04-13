using System.Collections.Generic;

namespace Inspector
{
    public interface IFilter<T>
    {
        IEnumerable<T> Get();
    }
}
