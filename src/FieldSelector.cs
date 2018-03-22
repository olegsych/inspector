using System;

namespace Inspector
{
    class FieldSelector
    {
        public delegate FieldSelector Constructor(IScope scope);

        public static readonly Constructor New = (IScope scope) =>
            throw new NotImplementedException();

        public virtual Field Select(Type fieldType = null, string fieldName = null) =>
            throw new NotImplementedException();
    }
}
