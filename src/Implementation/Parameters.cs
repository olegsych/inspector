using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Inspector.Implementation
{
    sealed class Parameters: IEnumerable<ParameterInfo>
    {
        public Parameters(MethodBase method) =>
            Method = method ?? throw new ArgumentNullException(nameof(method));

        public MethodBase Method { get; }

        public IEnumerator<ParameterInfo> GetEnumerator() {
            IEnumerable<ParameterInfo> parameters = Method.GetParameters();
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
