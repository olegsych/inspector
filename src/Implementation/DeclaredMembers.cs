using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    sealed class DeclaredMembers: IMembers, IDecorator<IMembers>
    {
        public DeclaredMembers(IMembers source, Type declaringType) {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        public IMembers Source { get; }

        public Type DeclaringType { get; }

        public IEnumerable<Constructor> Constructors() =>
            new DeclarationFilter<Constructor, ConstructorInfo>(Source.Constructors(), DeclaringType);

        public IEnumerable<Event> Events() =>
            new DeclarationFilter<Event, EventInfo>(Source.Events(), DeclaringType);

        public IEnumerable<Field> Fields() =>
            new DeclarationFilter<Field, FieldInfo>(Source.Fields(), DeclaringType);

        public IEnumerable<Method> Methods() =>
            new DeclarationFilter<Method, MethodInfo>(Source.Methods(), DeclaringType);

        public IEnumerable<Property> Properties() =>
            new DeclarationFilter<Property, PropertyInfo>(Source.Properties(), DeclaringType);
    }
}
