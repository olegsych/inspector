using System;
using System.Collections;
using System.Collections.Generic;

namespace Inspector.Implementation
{
    abstract class Filter<T>: IEnumerable<T>, IDecorator<IEnumerable<T>>
    {
        public Filter(IEnumerable<T> source) =>
            Source = source ?? throw new ArgumentNullException(nameof(source));

        public IEnumerable<T> Source { get; }

        public IEnumerator<T> GetEnumerator() =>
            Where().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        protected abstract IEnumerable<T> Where();
    }
}
