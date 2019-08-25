using System;

namespace Inspector.Implementation
{
    sealed class StaticScope: TypeScope
    {
        public StaticScope(Type type) : base(type) { }
    }
}
