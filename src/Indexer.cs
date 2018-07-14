using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to an indexer of type not accessible at compile time.
    /// </summary>
    public class Indexer : Member<PropertyInfo>
    {
        public Indexer(PropertyInfo info, object instance) : base(null, null) =>
            throw new NotImplementedException();

        public override bool IsStatic =>
            throw new NotImplementedException();

        public IndexerValue Value =>
            throw new NotImplementedException();

        public object Get(params object[] args) =>
            throw new NotImplementedException();

        public void Set(object value, params object[] args) =>
            throw new NotImplementedException();

        public class IndexerValue
        {
            public object this[params object[] args] {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }
        }
    }
}
