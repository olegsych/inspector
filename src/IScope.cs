namespace Inspector
{
    public interface IScope : IFilter<Constructor>, IFilter<Event>, IFilter<Field>, IFilter<Method>, IFilter<Property>
    {
    }
}
