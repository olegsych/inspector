using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    class AccessibilityScope : IScope, IDecorator<IScope>
    {
        public AccessibilityScope(IScope previous, Accessibility accessibility) {
            if (previous == null)
                throw new ArgumentNullException(nameof(previous));

            if(previous is AccessibilityScope scope) {
                Accessibility = Combine(scope.Accessibility, accessibility);
                Previous = scope.Previous;
            }
            else {
                Previous = previous;
                Accessibility = accessibility;
            }
        }

        public Accessibility Accessibility { get; }

        public IScope Previous { get; }

        string IDescriptor.Describe() => throw new NotImplementedException();

        IEnumerable<Constructor> IFilter<Constructor>.Get() => throw new NotImplementedException();
        IEnumerable<Event> IFilter<Event>.Get() => throw new NotImplementedException();

        IEnumerable<Field> IFilter<Field>.Get() =>
            ((IFilter<Field>)Previous).Get().Where(AccessibilityMatches);

        IEnumerable<Method> IFilter<Method>.Get() =>
            ((IFilter<Method>)Previous).Get().Where(AccessibilityMatches);

        IEnumerable<Property> IFilter<Property>.Get() => throw new NotImplementedException();

        bool AccessibilityMatches(Field field) =>
            Accessibility == (Accessibility)(field.Info.Attributes & FieldAttributes.FieldAccessMask);

        bool AccessibilityMatches(Method method) =>
            Accessibility == (Accessibility)(method.Info.Attributes & MethodAttributes.MemberAccessMask);

        static Accessibility Combine(Accessibility a1, Accessibility a2) {
            if(a1 == Accessibility.Private && a2 == Accessibility.Protected)
                return Accessibility.PrivateProtected;
            if(a1 == Accessibility.Protected && a2 == Accessibility.Internal)
                return Accessibility.ProtectedInternal;

            string error = $"'{a1.ToString().ToLower()} {a2.ToString().ToLower()}' is not a valid accessibility.";
            throw new InvalidOperationException(error);
        }
    }
}
