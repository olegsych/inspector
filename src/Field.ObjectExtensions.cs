using System;

namespace Inspector
{
    public static partial class ObjectExtensions
    {
        public static Field Field(this object instance, string fieldName) =>
           throw new NotImplementedException();

        public static Field Field(this object instance, Type fieldType, string fieldName = null) =>
            throw new NotImplementedException();

        public static Field<T> Field<T>(this object instance, string fieldName = null) =>
            throw new NotImplementedException();
    }
}
