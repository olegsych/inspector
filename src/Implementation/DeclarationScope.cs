using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    sealed class DeclarationScope: IScope, IDecorator<IScope>
    {
        public DeclarationScope(IScope source, Type declaringType) {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        public IScope Source { get; }

        public Type DeclaringType { get; }

        public IEnumerable<Constructor> Constructors() =>
            new DeclaredMembers<Constructor, ConstructorInfo>(Source.Constructors(), DeclaringType);

        public IEnumerable<Event> Events() =>
            new DeclaredMembers<Event, EventInfo>(Source.Events(), DeclaringType);

        public IEnumerable<Field> Fields() =>
            new DeclaredMembers<Field, FieldInfo>(Source.Fields(), DeclaringType);

        public IEnumerable<Method> Methods() =>
            new DeclaredMembers<Method, MethodInfo>(Source.Methods(), DeclaringType);

        public IEnumerable<Property> Properties() =>
            new DeclaredMembers<Property, PropertyInfo>(Source.Properties(), DeclaringType);
    }
}
