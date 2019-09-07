using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    sealed class FieldTypeFilter: Filter<Field>
    {
        public FieldTypeFilter(IEnumerable<Field> source, Type fieldType) : base(source) =>
            FieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));

        public Type FieldType { get; }

        protected override IEnumerable<Field> Where() =>
            Source.Where(field => field.Info.FieldType == FieldType);
    }
}
