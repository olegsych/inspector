namespace Inspector.Implementation
{
    interface IDecorator<out T>
    {
        T Source { get; }
    }
}
