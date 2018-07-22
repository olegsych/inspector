using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a field of type not accessible at compile time.
    /// </summary>
    public class Field : Member<FieldInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        public Field(FieldInfo info, object instance = null) : base(info, instance) {
            if(info.IsStatic) {
                if(instance != null)
                    throw new ArgumentException($"Instance shouldn't be specified for static field {info.Name}.", nameof(instance));
            }
            else {
                if(instance == null)
                    throw new ArgumentNullException(nameof(instance), $"Instance is required for field {info.Name}.");
            }
        }

        internal static Field Create(FieldInfo info, object instance) =>
            new Field(info, instance);

        /// <summary>
        /// Returns <c>true</c> when the <see cref="Field"/> is static.
        /// </summary>
        public override bool IsStatic =>
            Info.IsStatic;

        /// <summary>
        /// Gets or sets the field value.
        /// </summary>
        public object Value {
            get => Get();
            set => Set(value);
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        public object Get() =>
            Info.GetValue(Instance);

        /// <summary>
        /// Sets the field value.
        /// </summary>
        public void Set(object value) =>
            Info.SetValue(Instance, value);
    }
}
