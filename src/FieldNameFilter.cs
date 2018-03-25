using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    sealed class FieldNameFilter : IFilter<Field>, IDecorator<IFilter<Field>>
    {
        readonly string fieldName;

        public FieldNameFilter(IFilter<Field> previous, string fieldName) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            this.fieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        }

        public IFilter<Field> Previous { get; }

        string IDescriptor.Describe() =>
            throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            Previous.Get().Where(field => field.Info.Name == fieldName);
    }
}
