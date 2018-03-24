using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    [Collection(nameof(FieldFixture))]
    public class FieldFixture : IDisposable
    {
        readonly FieldInfo select = typeof(Field).GetField(nameof(Field.Select), BindingFlags.NonPublic | BindingFlags.Static);
        readonly Field.Selector original = Field.Select;

        internal readonly Field.Selector selector = Substitute.For<Field.Selector>();

        public FieldFixture() {
            original = (Field.Selector)select.GetValue(null);
            select.SetValue(null, selector);
        }

        public void Dispose() =>
            select.SetValue(null, original);
    }
}
