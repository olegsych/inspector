using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    sealed class FieldTypeFilter: Filter<Field>
    {
        public FieldTypeFilter(IEnumerable<Field> previous, Type fieldType) : base(previous) =>
            FieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));

        public Type FieldType { get; }

        public override IEnumerator<Field> GetEnumerator() =>
            Previous.Where(field => field.Info.FieldType == FieldType).GetEnumerator();
    }
}
