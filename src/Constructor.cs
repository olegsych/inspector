using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a constructor with signature not accessible at compile time.
    /// </summary>
    public class Constructor : Member<ConstructorInfo>
    {
        /// <summary>
        /// Initializes a new method of the <see cref="Constructor"/> class.
        /// </summary>
        public Constructor(ConstructorInfo info, object instance = null) : base(info, instance) {
            if(info.IsStatic && instance != null)
                throw new ArgumentException("Static constructor cannot be used with an instance.", nameof(instance));
        }

        /// <summary>
        /// Invokes the constructor with given <paramref name="parameters"/>.
        /// </summary>
        public object Invoke(params object[] parameters) {
            if(Info.IsStatic)
                return Info.Invoke(null, parameters);

            if(Instance != null)
                return Info.Invoke(Instance, parameters);

            return Info.Invoke(parameters);
        }

        internal static Constructor Create(ConstructorInfo info, object instance) =>
            new Constructor(info, instance);
    }
}
