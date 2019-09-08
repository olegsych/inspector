using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    public static class PropertyExtensions
    {
        #region Object

        public static Property Property(this object instance) =>
            new InstanceMembers(instance).Property();

        public static Property Property(this object instance, string propertyName) =>
            new InstanceMembers(instance).Property(propertyName);

        public static Property Property(this object instance, Type propertyType) =>
            new InstanceMembers(instance).Property(propertyType);

        public static Property Property(this object instance, Type propertyType, string propertyName) =>
            new InstanceMembers(instance).Property(propertyType, propertyName);

        public static Property<T> Property<T>(this object instance) =>
            new InstanceMembers(instance).Property<T>();

        public static Property<T> Property<T>(this object instance, string propertyName) =>
            new InstanceMembers(instance).Property<T>(propertyName);

        #endregion

        #region Type

        public static Property Property(this Type type) =>
            new StaticMembers(type).Property();

        public static Property Property(this Type type, string propertyName) =>
            new StaticMembers(type).Property(propertyName);

        public static Property Property(this Type type, Type propertyType) =>
            new StaticMembers(type).Property(propertyType);

        public static Property Property(this Type type, Type propertyType, string propertyName) =>
            new StaticMembers(type).Property(propertyType, propertyName);

        public static Property<T> Property<T>(this Type type) =>
            new StaticMembers(type).Property<T>();

        public static Property<T> Property<T>(this Type type, string propertyName) =>
            new StaticMembers(type).Property<T>(propertyName);

        #endregion
    }
}
