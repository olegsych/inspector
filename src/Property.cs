using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a property of type not accessible at compile time.
    /// </summary>
    public class Property : Member<PropertyInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        public Property(PropertyInfo info, object instance) :
            base(info, instance) { }

        internal static Property Create(PropertyInfo property, object instance) =>
            new Property(property, instance);

        /// <summary>
        /// Returns <c>true</c> if the <see cref="Property"/> is static.
        /// </summary>
        public override bool IsStatic =>
            Info.GetMethod.IsStatic;

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public object Value {
            get => Get();
            set => Set(value);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public object Get() =>
            Info.GetValue(Instance);

        /// <summary>
        /// Sets the property value.
        /// </summary>
        public void Set(object value) =>
            Info.SetValue(Instance, value);
    }
}
