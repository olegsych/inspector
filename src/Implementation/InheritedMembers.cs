using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Implementation
{
    sealed class InheritedMembers: IMembers, IDecorator<IMembers>
    {
        public InheritedMembers(IMembers source, Type ancestorType) {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            AncestorType = ancestorType ?? throw new ArgumentNullException(nameof(ancestorType));
        }

        public IMembers Source { get; }

        public Type AncestorType { get; }

        public IEnumerable<Constructor> Constructors() =>
            Source.Constructors().Where(c => c.Info.DeclaringType.IsAssignableFrom(AncestorType));

        public IEnumerable<Event> Events() =>
            Source.Events().Where(e => e.Info.DeclaringType.IsAssignableFrom(AncestorType));

        public IEnumerable<Field> Fields() =>
            Source.Fields().Where(f => f.Info.DeclaringType.IsAssignableFrom(AncestorType));

        public IEnumerable<Method> Methods() =>
            Source.Methods().Where(m => m.Info.DeclaringType.IsAssignableFrom(AncestorType));

        public IEnumerable<Property> Properties() =>
            Source.Properties().Where(p => p.Info.DeclaringType.IsAssignableFrom(AncestorType));
    }
}
