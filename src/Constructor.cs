using System;
using System.Reflection;

namespace Inspector
{
    public class Constructor : Member<ConstructorInfo>
    {
        public object Invoke(params object[] args) =>
            throw new NotImplementedException();
    }

    public class Constructor<TSignature> : Constructor
    {
        public new TSignature Invoke =>
            throw new NotImplementedException();
    }
}
