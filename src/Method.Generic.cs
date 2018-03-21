using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a method with signature <typeparamref name="TSignature"/>.
    /// </summary>
    /// <typeparam name="TSignature">A <see cref="Delegate"/> that represents method signature.</typeparam>
    public class Method<TSignature> : Method
    {
        public Method(MethodInfo info, object instance) : base(null, null) =>
            throw new NotImplementedException();

        public new TSignature Invoke => throw new NotImplementedException();

        public static implicit operator TSignature(Method<TSignature> method) =>
            throw new NotImplementedException();
    }
}
