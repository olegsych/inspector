using System;
using System.Reflection;

namespace Inspector
{
    public class Method : Member<MethodInfo>
    {
        protected Method(MethodInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public object Invoke(params object[] arga) => throw new NotImplementedException();
    }

    public class Method<TSignature> : Method
    {
        protected Method(MethodInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public new TSignature Invoke => throw new NotImplementedException();

        public static implicit operator TSignature(Method<TSignature> method) =>
            throw new NotImplementedException();
    }
}
