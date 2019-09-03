using System;
using System.Collections;
using System.Collections.Generic;

namespace Inspector.Implementation
{
    abstract class Filter<T>: IEnumerable<T>, IDecorator<IEnumerable<T>>
    {
        public Filter(IEnumerable<T> previous) =>
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));

        public IEnumerable<T> Previous { get; }

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
