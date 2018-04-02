using System;
using System.Collections.Generic;

namespace Inspector
{
    public interface IScope : IFilter<Constructor>, IFilter<Event>, IFilter<Field>, IFilter<Method>, IFilter<Property>
    {
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
