using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to an indexer of type not accessible at compile time.
    /// </summary>
    public class Indexer: Member<PropertyInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Indexer"/> class.
        /// </summary>
        public Indexer(PropertyInfo info, object instance) : base(null!, null) =>
            throw new NotImplementedException();

        /// <summary>
        /// Gets a value that indicates whether the indexer is static.
        /// </summary>
        public override bool IsStatic =>
            throw new NotImplementedException();

        /// <summary>
        /// Gets the indexer value accessor.
        /// </summary>
        public IndexerValue Value =>
            throw new NotImplementedException();

        /// <summary>
        /// Returns the indexer value for the specified arguments.
        /// </summary>
        public object Get(params object[] args) =>
            throw new NotImplementedException();

        /// <summary>
        /// Sets the indexer value for the specified arguments.
        /// </summary>
        public void Set(object value, params object[] args) =>
            throw new NotImplementedException();

        /// <summary>
        /// Provides indexed access to a value using array-style syntax.
        /// </summary>
        public class IndexerValue
        {
            /// <summary>
            /// Gets or sets the value at the specified index.
            /// </summary>
            public object this[params object[] args] {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }
        }
    }
}
