using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class FieldNameFilter : IFilter<Field>, IDecorator<IFilter<Field>>
    {
        public FieldNameFilter(IFilter<Field> previous, string fieldName) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        }

        public IFilter<Field> Previous { get; }

        public string FieldName { get; }

        IEnumerable<Field> IFilter<Field>.Get() =>
            Previous.Get().Where(field => field.Info.Name == FieldName);
    }
}
