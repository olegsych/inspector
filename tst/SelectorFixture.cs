using System;
using System.Collections.Generic;
using System.Reflection;
using Inspector.Implementation;
using NSubstitute;

namespace Inspector
{
    public class SelectorFixture<T>: IDisposable
    {
        readonly FieldInfo field = typeof(Selector<T>).GetField(nameof(Selector<T>.Select), BindingFlags.NonPublic | BindingFlags.Static)!;
        readonly Func<IEnumerable<T>, T> original = Selector<T>.Select;

        internal readonly Func<IEnumerable<T>, T> select = Substitute.For<Func<IEnumerable<T>, T>>();

        public SelectorFixture() {
            original = (Func<IEnumerable<T>, T>)field.GetValue(null!)!;
            field.SetValue(null, select);
        }

        public void Dispose() =>
            field.SetValue(null, original);
    }
}
