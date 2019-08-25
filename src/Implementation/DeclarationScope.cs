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

        IEnumerable<Constructor> IFilter<Constructor>.Get() =>
            Get<Constructor, ConstructorInfo>();

        IEnumerable<Event> IFilter<Event>.Get() =>
            Get<Event, EventInfo>();

        IEnumerable<Field> IFilter<Field>.Get() =>
            Get<Field, FieldInfo>();

        IEnumerable<Method> IFilter<Method>.Get() =>
            Get<Method, MethodInfo>();

        IEnumerable<Property> IFilter<Property>.Get() =>
            Get<Property, PropertyInfo>();

        IEnumerable<TMember> Get<TMember, TInfo>() where TMember : Member<TInfo> where TInfo : MemberInfo =>
            ((IFilter<TMember>)Previous).Get().Where(c => c.Info.DeclaringType == DeclaringType);
    }
}
