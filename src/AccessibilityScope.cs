using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    class AccessibilityScope : IScope, IDecorator<IScope>
    {
        public AccessibilityScope(IScope previous, Accessibility accessibility) {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            Accessibility = accessibility;
        }

        public Accessibility Accessibility { get; }

        public IScope Previous { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();

        IEnumerable<Constructor> IFilter<Constructor>.Get() => throw new NotImplementedException();
        IEnumerable<Event> IFilter<Event>.Get() => throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            ((IFilter<Field>)Previous).Get().Where(AccessibilityMatches);

        IEnumerable<Method> IFilter<Method>.Get() => throw new NotImplementedException();
        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();

        bool AccessibilityMatches(Field field) =>
            Accessibility == (Accessibility)(field.Info.Attributes & FieldAttributes.FieldAccessMask);
    }
}
