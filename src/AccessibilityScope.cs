using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    class AccessibilityScope : IScope, IDecorator<IScope>
    {
        public AccessibilityScope(IScope previous, IEnumerable<AccessModifier> accessModifiers) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            AccessModifiers = accessModifiers ?? throw new ArgumentNullException(nameof(accessModifiers));
        }

        public IEnumerable<AccessModifier> AccessModifiers { get; }

        public IScope Previous { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();

        IEnumerable<Constructor> IFilter<Constructor>.Get() => throw new NotImplementedException();
        IEnumerable<Event> IFilter<Event>.Get() => throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            ((IFilter<Field>)Previous).Get().Where(AccessibilityMatches);

        IEnumerable<Method> IFilter<Method>.Get() => throw new NotImplementedException();
        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();

        bool AccessibilityMatches(Field field) {
            FieldAttributes accessibility = field.Info.Attributes & FieldAttributes.FieldAccessMask;
            return AccessModifiers.Contains((AccessModifier)accessibility);
        }
    }
}
