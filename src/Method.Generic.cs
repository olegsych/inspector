using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Provides access to a method with signature <typeparamref name="TSignature"/>.
    /// </summary>
    /// <typeparam name="TSignature">A <see cref="Delegate"/> that represents method signature.</typeparam>
    public class Method<TSignature>: Method where TSignature : Delegate
    {
        readonly TSignature @delegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Method{TSignature}"/> class.
        /// </summary>
        internal Method(Method method, IDelegateFactory<MethodInfo> delegateFactory) : base(NotNull(method).Info, method.Instance) {
            if(!delegateFactory.TryCreate(Instance, Info, out @delegate!)) {
                string error = $"Method {method.Info} doesn't match expected signature.";
                throw new ArgumentException(error, nameof(method));
            }
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        public new TSignature Invoke => @delegate;

        /// <summary>
        /// Implicitly converts the method to its <typeparamref name="TSignature"/> delegate.
        /// </summary>
        #if NETSTANDARD2_1
        [return: NotNullIfNotNull("method")]
        #endif
        public static implicit operator TSignature?(Method<TSignature>? method) =>
            (method != null) ? method.Invoke : default;

        static Method NotNull(Method method) =>
            method ?? throw new ArgumentNullException(nameof(method));
    }
}
