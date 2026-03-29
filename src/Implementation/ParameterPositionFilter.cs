using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector.Implementation
{
    static class ParameterPositionFilter
    {
        public static IEnumerable<ParameterInfo> WithPosition(this IEnumerable<ParameterInfo> parameters, int position) =>
            new Implementation(parameters, position);

        internal sealed class Implementation: Filter<ParameterInfo>
        {
            internal Implementation(IEnumerable<ParameterInfo> parameters, int position) : base(parameters) =>
                Position = position;

            public int Position { get; }

            protected override IEnumerable<ParameterInfo> Where() =>
                Source.Where(parameter => parameter.Position == Position);
        }
    }
}
