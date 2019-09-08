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
        #region IMembers

        public static Field Field(this IMembers members) =>
            members.Fields().Single();

        public static Field Field(this IMembers members, string fieldName) =>
            new MemberNameFilter<Field, FieldInfo>(members.Fields(), fieldName).Single();

        public static Field Field(this IMembers members, Type fieldType) =>
            new FieldTypeFilter(members.Fields(), fieldType).Single();

        public static Field Field(this IMembers members, Type fieldType, string fieldName) {
            var typed = new FieldTypeFilter(members.Fields(), fieldType);
            var named = new MemberNameFilter<Field, FieldInfo>(typed, fieldName);
            return named.Single();
        }

        public static Field<T> Field<T>(this IMembers members) =>
            new Field<T>(members.Field(typeof(T)));

        public static Field<T> Field<T>(this IMembers members, string fieldName) =>
            new Field<T>(members.Field(typeof(T), fieldName));

        #endregion

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

        #region Type

        public static Field Field(this Type type) =>
            new StaticMembers(type).Field();

        public static Field Field(this Type type, Type fieldType) =>
            new StaticMembers(type).Field(fieldType);

        public static Field Field(this Type type, string fieldName) =>
            new StaticMembers(type).Field(fieldName);

        public static Field Field(this Type type, Type fieldType, string fieldName) =>
            new StaticMembers(type).Field(fieldType, fieldName);

        public static Field<T> Field<T>(this Type type) =>
            new StaticMembers(type).Field<T>();

        public static Field<T> Field<T>(this Type type, string fieldName) =>
            new StaticMembers(type).Field<T>(fieldName);

        #endregion
    }
}
