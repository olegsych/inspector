using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    sealed class PropertyTypeFilter: Filter<Property>
    {
        public PropertyTypeFilter(IEnumerable<Property> source, Type propertyType) : base(source) =>
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));

        public Type PropertyType { get; }

        public override IEnumerator<Property> GetEnumerator() =>
            Source.Where(p => p.Info.PropertyType == PropertyType).GetEnumerator();
    }
}
