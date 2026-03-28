using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a field of type not accessible at compile time.
    /// </summary>
    public class Field: Member<FieldInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        public Field(FieldInfo info, object? instance = null) :
            base(info, instance) { }

        internal static Field Create(FieldInfo info, object? instance) =>
            new Field(info, instance);

        /// <summary>
        /// Gets a value that indicates whether the field is static.
        /// </summary>
        public override bool IsStatic =>
            Info.IsStatic;

        /// <summary>
        /// Gets or sets the field value.
        /// </summary>
        public object? Value {
            get => Get();
            set => Set(value);
        }

        /// <summary>
        /// Returns the field value.
        /// </summary>
        public object? Get() =>
            Info.GetValue(Instance);

        /// <summary>
        /// Sets the field value.
        /// </summary>
        public void Set(object? value) =>
            Info.SetValue(Instance, value);
    }
}
