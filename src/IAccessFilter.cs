namespace Inspector
{
    /// <summary>
    /// Represents a pattern that <see cref="ObjectInspector"/> and <see cref="TypeInspector"/> should implement.
    /// This is a design concept only. Remove, unless it's actually used.
    /// </summary>
    interface IAccessFilter<T> where T : IAccessFilter<T>
    {
        T Public();
        T Protected();
        T Internal();
        T Private();
    }
}
