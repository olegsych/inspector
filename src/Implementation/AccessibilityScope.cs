using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class AccessibilityScope: IScope, IDecorator<IScope>
    {
        public AccessibilityScope(IScope source, Accessibility accessibility) {
            if(source == null)
                throw new ArgumentNullException(nameof(source));

            if(source is AccessibilityScope scope) {
                Accessibility = Combine(scope.Accessibility, accessibility);
                Source = scope.Source;
            }
            else {
                Source = source;
                Accessibility = accessibility;
            }
        }

        public Accessibility Accessibility { get; }

        public IScope Source { get; }

        public IEnumerable<Constructor> Constructors() =>
            Source.Constructors().Where(c => Accessibility == (Accessibility)(c.Info.Attributes & MethodAttributes.MemberAccessMask));

        public IEnumerable<Event> Events() =>
            Source.Events().Where(e => Accessibility == (Accessibility)(e.Info.AddMethod.Attributes & MethodAttributes.MemberAccessMask));

        public IEnumerable<Field> Fields() =>
            Source.Fields().Where(f => Accessibility == (Accessibility)(f.Info.Attributes & FieldAttributes.FieldAccessMask));

        public IEnumerable<Method> Methods() =>
            Source.Methods().Where(m => Accessibility == (Accessibility)(m.Info.Attributes & MethodAttributes.MemberAccessMask));

        public IEnumerable<Property> Properties() =>
            Source.Properties().Where(p => Accessibility == (Accessibility)(p.Info.GetMethod.Attributes & MethodAttributes.MemberAccessMask));

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
