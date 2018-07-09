using System;
using System.Collections.Generic;

namespace Inspector
{
    public interface IScope : IFilter<Constructor>, IFilter<Event>, IFilter<Field>, IFilter<Method>, IFilter<Property>
    {
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
