using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Provides access to a constructor with signature not accessible at compile time.
    /// </summary>
    public class Constructor : Member<ConstructorInfo>
    {
        /// <summary>
        /// Initializes a new method of the <see cref="Constructor"/> class.
        /// </summary>
        public Constructor(ConstructorInfo info, object instance = null) :
            base(info, instance) { }

        /// <summary>
        /// Returns <c>true</c> when the <see cref="Constructor"/> is static.
        /// </summary>
        public override bool IsStatic =>
            Info.IsStatic;

        /// <summary>
        /// Invokes the constructor with given <paramref name="parameters"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is void because constructors don't allocate types or instances; they only initialize them.
        /// The <see cref="MethodBase.Invoke(object, object[])"/> method returns <c>null</c> for both
        /// static and instance constructors. While the <see cref="ConstructorInfo.Invoke(object[])"/>
        /// method does create and returns an initialized instance, it's only a convenience method and,
        /// arguably, one that makes understanding constructors more confusing.
        /// </para>
        /// <para>
        /// To create a new instance of a given type, use <see cref="TypeExtensions.New(Type, object[])"/>.
        /// </para>
        /// </remarks>
        public void Invoke(params object[] parameters) =>
            Info.Invoke(Instance, parameters);

        internal static Constructor Create(ConstructorInfo info, object instance) =>
            new Constructor(info, instance);
    }
}
