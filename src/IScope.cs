using System;
using System.Collections;
using System.Collections.Generic;

namespace Inspector
{
    public interface IScope : IFilter<Constructor>, IFilter<Event>, IFilter<Field>, IFilter<Method>, IFilter<Property>
    {
    }

    enum Access
    {
        Private = 1,
        PrivateProtected = 2,
        Internal = 3,
        Protected = 4,
        ProtectedInternal = 5,
        Public = 6
    }

    class AccessScope : IScope
    {
        public AccessScope(IScope previous) => throw new NotImplementedException();

        string IDescriptor.Describe() => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Constructor> IEnumerable<Constructor>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Event> IEnumerable<Event>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Field> IEnumerable<Field>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Method> IEnumerable<Method>.GetEnumerator() => throw new NotImplementedException();
        IEnumerator<Property> IEnumerable<Property>.GetEnumerator() => throw new NotImplementedException();
    }

    public static class ScopeConstructorExtensions
    {
        public static Constructor Constructor(this IScope scope, Type signatureDelegate) =>
            throw new NotImplementedException();

        public static Constructor Constructor(this IScope scope, Type[] parameterTypes) =>
            throw new NotImplementedException();

        public static Constructor<TSignature> Constructor<TSignature>(this IScope scope) =>
            throw new NotImplementedException();
    }

    public static class ScopeEventExtensions
    {
        public static Event Event(this IScope scope, string eventName) =>
            throw new NotImplementedException();

        public static Event Event(this IScope scope, Type signatureDelegate, string eventName = null) =>
            throw new NotImplementedException();

        public static Event<THandler> Event<THandler>(this IScope scope, string eventName = null) =>
            throw new NotImplementedException();
    }

    public static class ScopeMethodExtensions
    {
        public static Method Method(this IScope scope, string methodName) =>
            throw new NotImplementedException();

        public static Method Method(this IScope scope, Type signatureDelegate, string methodName = null) =>
            throw new NotImplementedException();

        public static Method<TSignature> Method<TSignature>(this IScope scope, string methodName = null) =>
            throw new NotImplementedException();
    }

    public static class ScopePropertyExtensions
    {
        public static Property Property(this IScope scope, string propertyName) =>
            throw new NotImplementedException();

        public static Property Property(this IScope scope, Type propertyType, string propertyName = null) =>
            throw new NotImplementedException();

        public static Field<T> Property<T>(this IScope scope, string propertyName = null) =>
            throw new NotImplementedException();
    }
}
