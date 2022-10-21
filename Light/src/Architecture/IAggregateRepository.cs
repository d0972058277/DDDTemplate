namespace Architecture
{
    public interface IAggregateRepository<T> : IRepository where T : IAggregateRoot { }
}