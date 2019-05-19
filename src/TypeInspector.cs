using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    class TypeInspector
    {
        readonly TypeInfo type;

        protected TypeInspector(TypeInfo type) =>
            this.type = type;

        protected TypeInspector() { }

        public delegate TypeInspector Factory(Type declaredType, object instance = null);

        public static readonly Factory Create = (Type declaredType, object instance) => {
            Type type = instance?.GetType() ?? declaredType;
            return new TypeInspector(type.GetTypeInfo());
        };

        public virtual ConstructorInfo GetConstructor(params Type[] parameterTypes) {
            if(parameterTypes == null)
                throw new ArgumentNullException(nameof(parameterTypes));

            ConstructorInfo constructor = GetConstructors()
                .Where(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes))
                .SingleOrDefault();

            if(constructor == null) {
                var parameterTypeNames = string.Join(", ", parameterTypes.Select(p => p.Name));
                string message = $"Type {type.Name} doesn't have a constructor with parameter types({parameterTypeNames})";
                throw new ArgumentException(message, nameof(parameterTypes));
            }

            return constructor;
        }

        public virtual IReadOnlyList<ConstructorInfo> GetConstructors() =>
            type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }
}
