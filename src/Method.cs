using System;
using System.Reflection;

namespace Inspector
{
    public class Method : Member<MethodInfo>
    {
        public object Invoke(params object[] arga) => throw new NotImplementedException();
    }

    public class Method<TSignature> : Method
    {
        public new TSignature Invoke => throw new NotImplementedException();

        public static implicit operator TSignature(Method<TSignature> method) =>
            throw new NotImplementedException();
    }
}
