using System.Collections.Generic;

namespace Inspector
{
    public interface IFilter<T> : IDescriptor
    {
        IEnumerable<T> Get();
    }
}
