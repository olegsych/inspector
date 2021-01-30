using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Inspector.Implementation;

namespace Inspector
{
    /// <summary>
    /// Provides access to a constructor with signature known at compile time.
    /// </summary>
    /// <typeparam name="TSignature">A <see cref="Delegate"/> representing the constructor signature.</typeparam>
    public class Constructor<TSignature>: Constructor where TSignature : Delegate
    {
        readonly TSignature @delegate;

        internal Constructor(Constructor constructor, IDelegateFactory<ConstructorInfo> delegateFactory) :
            base(NotNull(constructor).Info, constructor.Instance) {

            if(!delegateFactory.TryCreate(Instance, Info, out @delegate!)) {
                string error = $"Constructor {constructor.Info} doesn't match expected signature.";
                throw new ArgumentException(error, nameof(constructor));
            }
        }

        /// <summary>
        /// Invokes the constructor.
        /// </summary>
        public new TSignature Invoke => @delegate;

        /// <summary>
        /// Implicitly converts the constructor to its <typeparamref name="TSignature"/> delegate.
        /// </summary>
        #if NETSTANDARD2_1
        [return:NotNullIfNotNull("constructor")]
        #endif
        public static implicit operator TSignature?(Constructor<TSignature>? constructor) =>
            constructor?.Invoke;

        static Constructor NotNull(Constructor constructor) =>
            constructor ?? throw new ArgumentNullException(nameof(constructor));
    }
}
