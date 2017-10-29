using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inspector
{
    public class TypeInspector
    {
        readonly TypeInfo type;

        protected TypeInspector(TypeInfo type) => this.type = type;

        protected TypeInspector()
        {
        }

        public delegate TypeInspector Factory(object instance, Type defaultType);

        public static readonly Factory Create = (object instance, Type defaultType) =>
        {
            Type type = instance?.GetType() ?? defaultType;
            return new TypeInspector(type.GetTypeInfo());
        };

        public virtual ConstructorInfo GetConstructor()
        {
            return GetConstructors().Single();
        }

        public virtual IReadOnlyList<ConstructorInfo> GetConstructors()
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}
