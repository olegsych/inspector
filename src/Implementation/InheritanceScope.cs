using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    sealed class InheritanceScope: IScope, IDecorator<IScope>
    {
        public InheritanceScope(IScope previous, Type ancestorType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            AncestorType = ancestorType ?? throw new ArgumentNullException(nameof(ancestorType));
        }

        public IScope Previous { get; }

        public Type AncestorType { get; }

        public IEnumerable<Constructor> Constructors() =>
            Previous.Constructors().Where(c => c.Info.DeclaringType.IsAssignableFrom(AncestorType));

        public IEnumerable<Event> Events() =>
            Previous.Events().Where(e => e.Info.DeclaringType.IsAssignableFrom(AncestorType));

        public IEnumerable<Field> Fields() =>
            Previous.Fields().Where(f => f.Info.DeclaringType.IsAssignableFrom(AncestorType));

        public IEnumerable<Method> Methods() =>
            Previous.Methods().Where(m => m.Info.DeclaringType.IsAssignableFrom(AncestorType));

        public IEnumerable<Property> Properties() =>
            Previous.Properties().Where(p => p.Info.DeclaringType.IsAssignableFrom(AncestorType));
    }
}
