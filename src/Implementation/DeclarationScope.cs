using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    sealed class DeclarationScope: IScope, IDecorator<IScope>
    {
        public DeclarationScope(IScope previous, Type declaringType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        public IScope Previous { get; }

        public Type DeclaringType { get; }

        public IEnumerable<Constructor> Constructors() =>
            new DeclaredMembers<Constructor, ConstructorInfo>(Previous.Constructors(), DeclaringType);

        public IEnumerable<Event> Events() =>
            new DeclaredMembers<Event, EventInfo>(Previous.Events(), DeclaringType);

        public IEnumerable<Field> Fields() =>
            new DeclaredMembers<Field, FieldInfo>(Previous.Fields(), DeclaringType);

        public IEnumerable<Method> Methods() =>
            new DeclaredMembers<Method, MethodInfo>(Previous.Methods(), DeclaringType);

        public IEnumerable<Property> Properties() =>
            new DeclaredMembers<Property, PropertyInfo>(Previous.Properties(), DeclaringType);
    }
}
