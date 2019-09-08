using System.Collections.Generic;

namespace Inspector
{
    /// <summary>
    /// Provides access to members of an object type or instance.
    /// </summary>
    public interface IMembers
    {
        IEnumerable<Constructor> Constructors();
        IEnumerable<Event> Events();
        IEnumerable<Field> Fields();
        IEnumerable<Method> Methods();
        IEnumerable<Property> Properties();
    }
}
