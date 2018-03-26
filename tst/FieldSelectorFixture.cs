using Xunit;

namespace Inspector
{
    /// <summary>
    /// Base class for tests that need to substitute static <see cref="Selector{Field}.Select"/> method.
    /// </summary>
    [Collection(nameof(FieldSelectorFixture))]
    public class FieldSelectorFixture : SelectorFixture<Field>
    {
    }
}
