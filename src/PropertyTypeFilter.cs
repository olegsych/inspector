using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    /// <summary>
    /// Filters properties by type.
    /// </summary>
    sealed class PropertyTypeFilter : IFilter<Property>, IDecorator<IFilter<Property>>
    {
        public PropertyTypeFilter(IFilter<Property> previous, Type propertyType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
        }

        public IFilter<Property> Previous { get; }

        public Type PropertyType { get; }

        IEnumerable<Property> IFilter<Property>.Get() =>
            Previous.Get().Where(p => p.Info.PropertyType == PropertyType);
    }
}
