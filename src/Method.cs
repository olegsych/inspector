using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a static or instance method with signature not accessible at compile time.
    /// </summary>
    public class Method : Member<MethodInfo>
    {
        protected Method(MethodInfo info, object instance) : base(info, instance) =>
            throw new NotImplementedException();

        public object Invoke(params object[] arga) => throw new NotImplementedException();
    }
}
