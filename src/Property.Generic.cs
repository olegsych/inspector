using System;

namespace Inspector
{
    /// <summary>
    /// Provides access to a property of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of property value</typeparam>
    public class Property<T> : Property
    {
        internal Property(Property property) : base(NotNull(property).Info, property.Instance) {
            if(typeof(T) != Info.PropertyType)
                throw new ArgumentException($"Property type {Info.PropertyType.FullName} doesn't match expected {typeof(T).FullName}.", nameof(property));
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public new T Value {
            get => Get();
            set => Set(value);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public new T Get() =>
            (T)base.Get();

        /// <summary>
        /// Sets the property value.
        /// </summary>
        public void Set(T value) =>
            base.Set(value);

        /// <summary>
        /// Implicitly converts the property to it's value for convenient use in assertions.
        /// </summary>
        public static implicit operator T(Property<T> property) =>
            property != null ? property.Get() : default;

        static Property NotNull(Property property) =>
            property ?? throw new ArgumentNullException(nameof(property));
    }
}
