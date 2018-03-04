using System;

namespace Inspector
{
    public class ObjectInspector : IDeclarationFilter<ObjectInspector>, IAccessFilter<ObjectInspector> 
    {
        public Field<T> Field<T>(string fieldName = default) => throw new NotImplementedException();

        public ObjectInspector Public() => throw new NotImplementedException();

        public ObjectInspector Protected() => throw new NotImplementedException();

        public ObjectInspector Internal() => throw new NotImplementedException();

        public ObjectInspector Private() => throw new NotImplementedException();

        public ObjectInspector Declared(Type declaringType = default) => throw new NotImplementedException();

        public ObjectInspector Inherited(Type baseType = default) => throw new NotImplementedException();
    }
}
