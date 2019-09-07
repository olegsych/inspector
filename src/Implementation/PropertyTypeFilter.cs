using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    sealed class PropertyTypeFilter: Filter<Property>
    {
        public PropertyTypeFilter(IEnumerable<Property> previous, Type propertyType) : base(previous) =>
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));

        public Type PropertyType { get; }

        public override IEnumerator<Property> GetEnumerator() =>
            Previous.Where(p => p.Info.PropertyType == PropertyType).GetEnumerator();
    }
}
