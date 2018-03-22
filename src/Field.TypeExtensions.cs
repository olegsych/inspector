using System;

namespace Inspector
{
    public static partial class TypeExtensions
    {
        public static Field Field(this Type instance, string fieldName) =>
           throw new NotImplementedException();

        public static Field Field(this Type instance, Type fieldType, string fieldName = null) =>
            throw new NotImplementedException();

        public static Field<T> Field<T>(this Type instance, string fieldName = null) =>
            throw new NotImplementedException();
    }
}
