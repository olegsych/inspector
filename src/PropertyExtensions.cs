using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    public static class PropertyExtensions
    {
        #region IScope

        public static Property Property(this IScope scope) =>
            scope.Properties().Single();

        public static Property Property(this IScope scope, string propertyName) =>
            new MemberNameFilter<Property, PropertyInfo>(scope.Properties(), propertyName).Single();

        public static Property Property(this IScope scope, Type propertyType) =>
            new PropertyTypeFilter(scope.Properties(), propertyType).Single();

        public static Property Property(this IScope scope, Type propertyType, string propertyName) {
            var typed = new PropertyTypeFilter(scope.Properties(), propertyType);
            var named = new MemberNameFilter<Property, PropertyInfo>(typed, propertyName);
            return named.Single();
        }

        public static Property<T> Property<T>(this IScope scope) =>
            new Property<T>(scope.Property(typeof(T)));

        public static Property<T> Property<T>(this IScope scope, string propertyName) =>
            new Property<T>(scope.Property(typeof(T), propertyName));

        #endregion

        #region Object

        public static Property Property(this object instance) =>
            new InstanceScope(instance).Property();

        public static Property Property(this object instance, string propertyName) =>
            new InstanceScope(instance).Property(propertyName);

        public static Property Property(this object instance, Type propertyType) =>
            new InstanceScope(instance).Property(propertyType);

        public static Property Property(this object instance, Type propertyType, string propertyName) =>
            new InstanceScope(instance).Property(propertyType, propertyName);

        public static Property<T> Property<T>(this object instance) =>
            new InstanceScope(instance).Property<T>();

        public static Property<T> Property<T>(this object instance, string propertyName) =>
            new InstanceScope(instance).Property<T>(propertyName);

        #endregion

        #region Type

        public static Property Property(this Type type) =>
            new StaticScope(type).Property();

        public static Property Property(this Type type, string propertyName) =>
            new StaticScope(type).Property(propertyName);

        public static Property Property(this Type type, Type propertyType) =>
            new StaticScope(type).Property(propertyType);

        public static Property Property(this Type type, Type propertyType, string propertyName) =>
            new StaticScope(type).Property(propertyType, propertyName);

        public static Property<T> Property<T>(this Type type) =>
            new StaticScope(type).Property<T>();

        public static Property<T> Property<T>(this Type type, string propertyName) =>
            new StaticScope(type).Property<T>(propertyName);

        #endregion
    }
}
