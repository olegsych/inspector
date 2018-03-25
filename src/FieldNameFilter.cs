using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class FieldNameFilter : IFilter<Field>
    {
        readonly IFilter<Field> fields;
        readonly string fieldName;

        internal FieldNameFilter(IFilter<Field> fields, string fieldName) {
            this.fields = fields ?? throw new ArgumentNullException(nameof(fields));
            this.fieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        }

        string IDescriptor.Describe() =>
            throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            fields.Get().Where(field => field.Info.Name == fieldName);
    }
}
