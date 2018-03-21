using System;
using System.Reflection;

namespace Inspector
{
    public class Constructor<TSignature> : Constructor
    {
        public Constructor(ConstructorInfo info, object instance) : base(null, null) =>
            throw new NotImplementedException();

        public new TSignature Invoke =>
            throw new NotImplementedException();
    }
}
