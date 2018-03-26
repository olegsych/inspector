using System;

namespace Inspector
{
    public static class IScopeExtensions
    {
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

        public static Field<T> Field<T>(this IScope scope) {
            Field field = Field(scope, typeof(T));
            return new Field<T>(field.Info, field.Instance);
        }

        public static Field<T> Field<T>(this IScope scope, string fieldName) {
            Field field = Field(scope, typeof(T), fieldName);
            return new Field<T>(field.Info, field.Instance);
        }
    }
}
