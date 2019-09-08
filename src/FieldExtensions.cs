using System;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Extension methods for selecting fields.
    /// </summary>
    public static class FieldExtensions
    {
        #region Object

        public static Field Field(this object instance) =>
            new InstanceMembers(instance).Field();

        public static Field Field(this object instance, string fieldName) =>
            new InstanceMembers(instance).Field(fieldName);

        public static Field Field(this object instance, Type fieldType) =>
            new InstanceMembers(instance).Field(fieldType);

        public static Field Field(this object instance, Type fieldType, string fieldName) =>
            new InstanceMembers(instance).Field(fieldType, fieldName);

        public static Field<T> Field<T>(this object instance) =>
            new InstanceMembers(instance).Field<T>();

        public static Field<T> Field<T>(this object instance, string fieldName) =>
            new InstanceMembers(instance).Field<T>(fieldName);

        #endregion
    }
}
