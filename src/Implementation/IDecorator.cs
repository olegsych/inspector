namespace Inspector.Implementation
{
    interface IDecorator<out T>
    {
        T Previous { get; }
    }
}
