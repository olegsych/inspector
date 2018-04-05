using System;
using System.Reflection;

namespace Inspector
{
    /// <summary>
    /// Stores information required to access a static or an instance type member at runtime using Reflection.
    /// </summary>
    /// <typeparam name="TMemberInfo">
    /// A class derived from <see cref="MemberInfo"/> describing the type member.
    /// </typeparam>
    public abstract class Member<TMemberInfo> where TMemberInfo : MemberInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Member{TMemberInfo}"/> class.
        /// </summary>
        protected Member(TMemberInfo info, object instance) {
            Info = info ?? throw new ArgumentNullException(nameof(info));

            if(instance != null && !info.DeclaringType.GetTypeInfo().IsAssignableFrom(instance.GetType())) {
                string error = $"Instance type {instance.GetType().Name} doesn't match type {info.DeclaringType.Name} where {info.Name} is declared.";
                throw new ArgumentException(error, nameof(instance));
            }

            Instance = instance;
        }

        /// <summary>
        /// Gets the member metadata.
        /// </summary>
        public TMemberInfo Info { get; }

        /// <summary>
        /// Gets the object of the member or <c>null</c> if the member is static.
        /// </summary>
        public object Instance { get; }

        /// <summary>
        /// Implicitly converts a given member to its <see cref="MemberInfo"/> for convenient access to Reflection APIs.
        /// </summary>
        public static implicit operator TMemberInfo(Member<TMemberInfo> member) =>
            member?.Info;
    }
}
