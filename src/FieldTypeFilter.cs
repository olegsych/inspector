using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class FieldTypeFilter : IFilter<Field>, IDecorator<IFilter<Field>>
    {
        readonly Type fieldType;

        public FieldTypeFilter(IFilter<Field> previous, Type fieldType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            this.fieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));
        }

        public IFilter<Field> Previous { get; }

        string IDescriptor.Describe() =>
            throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            Previous.Get().Where(field => field.Info.FieldType == fieldType);
    }
}
