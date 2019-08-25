using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    sealed class InheritanceScope: IScope, IDecorator<IScope>
    {
        public InheritanceScope(IScope previous, Type ancestorType) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            AncestorType = ancestorType ?? throw new ArgumentNullException(nameof(ancestorType));
        }

        public IScope Previous { get; }

        public Type AncestorType { get; }

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
            ((IFilter<TMember>)Previous).Get().Where(m => m.Info.DeclaringType.IsAssignableFrom(AncestorType));
    }
}
