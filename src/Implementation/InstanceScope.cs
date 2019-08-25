namespace Inspector.Implementation
{
    sealed class InstanceScope: TypeScope
    {
        public InstanceScope(object instance) : base(instance) { }
    }
}
