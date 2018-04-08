using System;

namespace Inspector
{
    sealed class StaticScope : TypeScope
    {
        public StaticScope(Type type) : base(type) { }
    }
}
