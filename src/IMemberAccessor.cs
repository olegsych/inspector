namespace Inspector
{
    /// <summary>
    /// Represents a pattern that <see cref="ObjectInspector"/> and <see cref="TypeInspector"/> should implement.
    /// This is a design concept only. Remove, unless it's actually used.
    /// </summary>
    interface IMemberAccessor<TInspector> where TInspector : IMemberAccessor<TInspector>
    {
        Field<TFieldType> Field<TFieldType>(string fieldName = default);

        Property<TPropertyType> Property<TPropertyType>(string propertyName = default);
    }
}
