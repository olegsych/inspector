using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a static or instance method with signature not accessible at compile time.
    /// </summary>
    public class Method: Member<MethodInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        public Method(MethodInfo info, object? instance = null) :
            base(info, instance) { }

        /// <summary>
        /// Gets a value that indicates whether the method is static.
        /// </summary>
        public override bool IsStatic =>
            Info.IsStatic;

        internal static Method Create(MethodInfo info, object? instance) =>
            new Method(info, instance);

        /// <summary>
        /// Returns the result of invoking the method with given <paramref name="parameters"/>.
        /// </summary>
        public object Invoke(params object[] parameters) =>
            Info.Invoke(Instance, parameters);
    }
}
