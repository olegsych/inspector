using System.Collections.Generic;

namespace Inspector
{
    public interface IScope
    {
        IEnumerable<Constructor> Constructors();
        IEnumerable<Event> Events();
        IEnumerable<Field> Fields();
        IEnumerable<Method> Methods();
        IEnumerable<Property> Properties();
    }
}
