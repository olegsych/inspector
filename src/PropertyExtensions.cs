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
    }
}
