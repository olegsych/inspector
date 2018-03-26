using System;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector
{
    public static partial class TypeExtensions
    {
        public static ConstructorInfo Constructor<T>(this Type type) =>
            Constructor(type, typeof(T));

        public static ConstructorInfo Constructor<T1, T2>(this Type type) =>
            Constructor(type, typeof(T1), typeof(T2));

        public static ConstructorInfo Constructor<T1, T2, T3>(this Type type) =>
            Constructor(type, typeof(T1), typeof(T2), typeof(T3));

        public static ConstructorInfo Constructor<T1, T2, T3, T4>(this Type type) =>
            Constructor(type, typeof(T1), typeof(T2), typeof(T3), typeof(T4));

        public static ConstructorInfo Constructor<T1, T2, T3, T4, T5>(this Type type) =>
            Constructor(type, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

        public static ConstructorInfo Constructor<T1, T2, T3, T4, T5, T6>(this Type type) =>
            Constructor(type, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

        public static ConstructorInfo Constructor<T1, T2, T3, T4, T5, T6, T7>(this Type type) =>
            Constructor(type, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

        public static ConstructorInfo Constructor<T1, T2, T3, T4, T5, T6, T7, T8>(this Type type) =>
            Constructor(type, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

        public static ConstructorInfo Constructor(this Type type, params Type[] parameters) {
            var inspector = TypeInspector.Create(type);
            return inspector.GetConstructor(parameters);
        }

        public static IReadOnlyList<ConstructorInfo> Constructors(this Type type) {
            var inspector = TypeInspector.Create(type);
            return inspector.GetConstructors();
        }

        #region Field

        public static Field Field(this Type type) {
            var scope = new StaticScope(type);
            return Selector<Field>.Select(scope);
        }

        public static Field Field(this Type type, Type fieldType) {
            var scope = new StaticScope(type);
            var typed = new FieldTypeFilter(scope, fieldType);
            return Selector<Field>.Select(typed);
        }

        public static Field Field(this Type type, string fieldName) {
            var scope = new StaticScope(type);
            var named = new FieldNameFilter(scope, fieldName);
            return Selector<Field>.Select(named);
        }

        public static Field Field(this Type type, Type fieldType, string fieldName) {
            var scope = new StaticScope(type);
            var typed = new FieldTypeFilter(scope, fieldType);
            var named = new FieldNameFilter(typed, fieldName);
            return Selector<Field>.Select(named);
        }

        public static Field<T> Field<T>(this Type type) {
            Field field = Field(type, typeof(T));
            return new Field<T>(field.Info, field.Instance);
        }

        public static Field<T> Field<T>(this Type type, string fieldName) {
            Field field = Field(type, typeof(T), fieldName);
            return new Field<T>(field.Info, field.Instance);
        }

        #endregion

        public static object New(this Type type, params object[] parameters) =>
            throw new NotImplementedException();

        public static Property<T> Property<T>(this Type type, string name = default) =>
            throw new NotImplementedException();

        public static object Uninitialized(this Type type) =>
            throw new NotImplementedException();
    }
}
