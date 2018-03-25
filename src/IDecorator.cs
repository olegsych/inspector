namespace Inspector
{
    interface IDecorator<out T>
    {
        T Previous { get; }
    }
}
