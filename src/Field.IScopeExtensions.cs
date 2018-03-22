using System;

namespace Inspector
{
    public static partial class IScopeExtensions
    {
        public static Field Field(this IScope scope, string fieldName) =>
           throw new NotImplementedException();

        public static Field Field(this IScope scope, Type fieldType, string fieldName = null) =>
            throw new NotImplementedException();

        public static Field<T> Field<T>(this IScope scope, string fieldName = null) =>
            throw new NotImplementedException();
    }
}
