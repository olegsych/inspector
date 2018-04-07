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
        /// <summary>
        /// Initializes a new instance of the <see cref="Method{TSignature}"/> class.
        /// </summary>
        public Method(Method method) : base(NotNull(method).Info, method.Instance) {
            try {
                Invoke = (TSignature)(object)Delegate.CreateDelegate(typeof(TSignature), method.Instance, method.Info);
            }
            catch(ArgumentException e) {
                string error = $"Method {method.Info} doesn't match expected signature.";
                throw new ArgumentException(error, nameof(method), e);
            }
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        public new TSignature Invoke { get; }

        /// <summary>
        /// Implicitly converts the method to its <typeparamref name="TSignature"/> delegate.
        /// </summary>
        public static implicit operator TSignature(Method<TSignature> method) =>
            (method != null) ? method.Invoke : default;

        static Method NotNull(Method method) =>
            method ?? throw new ArgumentNullException(nameof(method));
    }
}
