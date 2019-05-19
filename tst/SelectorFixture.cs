using System;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Inspector
{
    public class SelectorFixture<T>: IDisposable
    {
        readonly FieldInfo field = typeof(Selector<T>).GetField(nameof(Selector<T>.Select), BindingFlags.NonPublic | BindingFlags.Static);
        readonly Func<IFilter<T>, T> original = Selector<T>.Select;

        internal readonly Func<IFilter<T>, T> select = Substitute.For<Func<IFilter<T>, T>>();

        public SelectorFixture() {
            original = (Func<IFilter<T>, T>)field.GetValue(null);
            field.SetValue(null, select);
        }

        public void Dispose() =>
            field.SetValue(null, original);
    }
}
