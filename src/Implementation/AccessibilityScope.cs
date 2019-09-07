using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class AccessibilityScope: IScope, IDecorator<IScope>
    {
        public AccessibilityScope(IScope previous, Accessibility accessibility) {
            if(previous == null)
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

        public IEnumerable<Constructor> Constructors() =>
            Previous.Constructors().Where(c => Accessibility == (Accessibility)(c.Info.Attributes & MethodAttributes.MemberAccessMask));

        public IEnumerable<Event> Events() =>
            Previous.Events().Where(e => Accessibility == (Accessibility)(e.Info.AddMethod.Attributes & MethodAttributes.MemberAccessMask));

        public IEnumerable<Field> Fields() =>
            Previous.Fields().Where(f => Accessibility == (Accessibility)(f.Info.Attributes & FieldAttributes.FieldAccessMask));

        public IEnumerable<Method> Methods() =>
            Previous.Methods().Where(m => Accessibility == (Accessibility)(m.Info.Attributes & MethodAttributes.MemberAccessMask));

        public IEnumerable<Property> Properties() =>
            Previous.Properties().Where(p => Accessibility == (Accessibility)(p.Info.GetMethod.Attributes & MethodAttributes.MemberAccessMask));

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
