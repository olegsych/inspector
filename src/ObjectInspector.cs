using System;

namespace Inspector
{
    public class ObjectInspector : IDeclarationFilter<ObjectInspector>, IAccessFilter<ObjectInspector> 
    {
        public Event Event(Type eventType = null, string eventName = null) => throw new NotImplementedException();

        public Field Field(Type fieldType = null, string fieldName = null) => throw new NotImplementedException();

        public Delegate Method(Type delegateType = null, string methodName = null) => throw new NotImplementedException();

        public Property Property(Type propertyType = null, string propertyName = null) => throw new NotImplementedException();

        public ObjectInspector Public() => throw new NotImplementedException();

        public ObjectInspector Protected() => throw new NotImplementedException();

        public ObjectInspector Internal() => throw new NotImplementedException();

        public ObjectInspector Private() => throw new NotImplementedException();

        public ObjectInspector Declared(Type declaringType = default) => throw new NotImplementedException();

        public ObjectInspector Inherited(Type baseType = default) => throw new NotImplementedException();
    }
}
