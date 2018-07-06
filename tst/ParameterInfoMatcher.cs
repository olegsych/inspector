using System;
using System.Reflection;

namespace Inspector
{
    static class ParameterInfoMatcher
    {
        internal static Func<ParameterInfo, ParameterInfo, bool> Match = (p1, p2) => {
            if (p1 == null)
                throw new ArgumentNullException(nameof(p1));
            if(p2 == null)
                throw new ArgumentNullException(nameof(p2));

            return p1.ParameterType == p2.ParameterType
                && p1.IsIn == p2.IsIn
                && p1.IsOut == p2.IsOut;
        };
    }
}
