using System;

namespace Inspector
{
    public static class IScopeExtensions
    {
        public static Field Field(this IScope scope, string fieldName = null) =>
           Inspector.Field.Select(scope, null, fieldName);

        public static Field Field(this IScope scope, Type fieldType, string fieldName = null) =>
            Inspector.Field.Select(scope, fieldType, fieldName);

        public static Field<T> Field<T>(this IScope scope, string fieldName = null) {
            Field field = Inspector.Field.Select(scope, typeof(T), fieldName);
            return new Field<T>(field.Info, field.Instance);
        }
    }
}
