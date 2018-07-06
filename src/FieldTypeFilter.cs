using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    /// <summary>
    /// Filters fields by type.
    /// </summary>
    sealed class FieldTypeFilter : IFilter<Field>, IDecorator<IFilter<Field>>
    {
        public FieldTypeFilter(IFilter<Field> previous, Type fieldType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            FieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));
        }

        public IFilter<Field> Previous { get; }

        public Type FieldType { get; }

        IEnumerable<Field> IFilter<Field>.Get() =>
            Previous.Get().Where(field => field.Info.FieldType == FieldType);
    }
}
