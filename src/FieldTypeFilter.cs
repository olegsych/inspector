using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class FieldTypeFilter : IFilter<Field>
    {
        readonly IFilter<Field> fields;
        readonly Type fieldType;

        internal FieldTypeFilter(IFilter<Field> fields, Type fieldType) {
            this.fields = fields ?? throw new ArgumentNullException(nameof(fields));
            this.fieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));
        }

        string IDescriptor.Describe() =>
            throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            fields.Get().Where(field => field.Info.FieldType == fieldType);
    }
}
