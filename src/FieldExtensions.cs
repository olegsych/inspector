using System;

namespace Inspector
{
    public static class FieldExtensions
    {
        #region IScope

        public static Field Field(this IScope scope) =>
            Selector<Field>.Select(scope);

        public static Field Field(this IScope scope, string fieldName) =>
            Selector<Field>.Select(new FieldNameFilter(scope, fieldName));

        public static Field Field(this IScope scope, Type fieldType) =>
            Selector<Field>.Select(new FieldTypeFilter(scope, fieldType));

        public static Field Field(this IScope scope, Type fieldType, string fieldName) {
            var typed = new FieldTypeFilter(scope, fieldType);
            var named = new FieldNameFilter(typed, fieldName);
            return Selector<Field>.Select(named);
        }

        public static Field<T> Field<T>(this IScope scope) =>
            new Field<T>(scope.Field(typeof(T)));

        public static Field<T> Field<T>(this IScope scope, string fieldName) =>
            new Field<T>(scope.Field(typeof(T), fieldName));

        #endregion

        #region Object

        public static Field Field(this object instance) =>
            new InstanceScope(instance).Field();

        public static Field Field(this object instance, string fieldName) =>
            new InstanceScope(instance).Field(fieldName);

        public static Field Field(this object instance, Type fieldType) =>
            new InstanceScope(instance).Field(fieldType);

        public static Field Field(this object instance, Type fieldType, string fieldName) =>
            new InstanceScope(instance).Field(fieldType, fieldName);

        public static Field<T> Field<T>(this object instance) =>
            new InstanceScope(instance).Field<T>();

        public static Field<T> Field<T>(this object instance, string fieldName) =>
            new InstanceScope(instance).Field<T>(fieldName);

        #endregion

        #region Type

        public static Field Field(this Type type) =>
            new StaticScope(type).Field();

        public static Field Field(this Type type, Type fieldType) =>
            new StaticScope(type).Field(fieldType);

        public static Field Field(this Type type, string fieldName) =>
            new StaticScope(type).Field(fieldName);

        public static Field Field(this Type type, Type fieldType, string fieldName) =>
            new StaticScope(type).Field(fieldType, fieldName);

        public static Field<T> Field<T>(this Type type) =>
            new StaticScope(type).Field<T>();

        public static Field<T> Field<T>(this Type type, string fieldName) =>
            new StaticScope(type).Field<T>(fieldName);

        #endregion
    }
}
