using System;
using System.Reflection;

namespace Inspector
{
    public class Constructor : Member<ConstructorInfo>
    {
        protected Constructor(ConstructorInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public object Invoke(params object[] args) =>
            throw new NotImplementedException();
    }

    public class Constructor<TSignature> : Constructor
    {
        protected Constructor(ConstructorInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public new TSignature Invoke =>
            throw new NotImplementedException();
    }
}
