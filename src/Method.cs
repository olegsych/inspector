using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a static or instance method with signature not accessible at compile time.
    /// </summary>
    public class Method : Member<MethodInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        public Method(MethodInfo info, object instance = null) : base(info, instance) {
            if (info.IsStatic) {
                if(instance != null)
                    throw new ArgumentException($"Instance shouldn't be specified for static method {info.Name}.", nameof(instance));
            }
            else {
                if(instance == null)
                    throw new ArgumentNullException(nameof(instance), $"Instance is required for method {info.Name}.");
            }
        }

        /// <summary>
        /// Returns <c>true</c> when the <see cref="Method"/> is static.
        /// </summary>
        public override bool IsStatic =>
            Info.IsStatic;

        internal static Method Create(MethodInfo info, object instance) =>
            new Method(info, instance);

        /// <summary>
        /// Invokes the method with specified <paramref name="parameters"/> and returns the value it returns.
        /// </summary>
        public object Invoke(params object[] parameters) =>
            Info.Invoke(Instance, parameters);
    }
}
