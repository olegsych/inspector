using System;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    public interface IScope
    {
        IEnumerable<Constructor> Constructors();
        IEnumerable<Event> Events();
        IEnumerable<Field> Fields();
        IEnumerable<Method> Methods();
        IEnumerable<Property> Properties();
    }

    class InstanceScope : IScope
    {
        public InstanceScope(object instance) => throw new NotImplementedException();

        IEnumerable<Constructor> IScope.Constructors() => throw new NotImplementedException();
        IEnumerable<Event> IScope.Events() => throw new NotImplementedException();
        IEnumerable<Field> IScope.Fields() => throw new NotImplementedException();
        IEnumerable<Method> IScope.Methods() => throw new NotImplementedException();
        IEnumerable<Property> IScope.Properties() => throw new NotImplementedException();
    }

    class StaticScope : IScope
    {
        public StaticScope(Type type) => throw new NotImplementedException();

        IEnumerable<Constructor> IScope.Constructors() => throw new NotImplementedException();
        IEnumerable<Event> IScope.Events() => throw new NotImplementedException();
        IEnumerable<Field> IScope.Fields() => throw new NotImplementedException();
        IEnumerable<Method> IScope.Methods() => throw new NotImplementedException();
        IEnumerable<Property> IScope.Properties() => throw new NotImplementedException();
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

        IEnumerable<Constructor> IScope.Constructors() => throw new NotImplementedException();
        IEnumerable<Event> IScope.Events() => throw new NotImplementedException();
        IEnumerable<Field> IScope.Fields() => throw new NotImplementedException();
        IEnumerable<Method> IScope.Methods() => throw new NotImplementedException();
        IEnumerable<Property> IScope.Properties() => throw new NotImplementedException();
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

    public static class ScopeFieldExtensions
    {
        public static Field Field(this IScope scope, string fieldName) =>
            throw new NotImplementedException();

        public static Field Field(this IScope scope, Type fieldType, string fieldName = null) =>
            throw new NotImplementedException();

        public static Field<T> Field<T>(this IScope scope, string fieldName = null) =>
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
