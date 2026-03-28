using System.Collections.Generic;

namespace Inspector
{
    /// <summary>
    /// Provides access to members of an object type or instance.
    /// </summary>
    public interface IMembers
    {
        /// <summary>
        /// Returns constructors.
        /// </summary>
        IEnumerable<Constructor> Constructors();

        /// <summary>
        /// Returns events.
        /// </summary>
        IEnumerable<Event> Events();

        /// <summary>
        /// Returns fields.
        /// </summary>
        IEnumerable<Field> Fields();

        /// <summary>
        /// Returns methods.
        /// </summary>
        IEnumerable<Method> Methods();

        /// <summary>
        /// Returns properties.
        /// </summary>
        IEnumerable<Property> Properties();
    }
}
