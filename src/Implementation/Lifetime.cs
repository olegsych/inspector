using System.Reflection;

namespace Inspector.Implementation
{
    /// <summary>
    /// Specifies lifetime of type members.
    /// Maps to <see cref="BindingFlags.Instance"/> and <see cref="BindingFlags.Static"/>.
    /// </summary>
    enum Lifetime
    {
        Static = BindingFlags.Static,
        Instance = BindingFlags.Instance
    }
}
