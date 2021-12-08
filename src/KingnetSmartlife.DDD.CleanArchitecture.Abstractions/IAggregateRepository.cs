namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IAggregateRepository<T> : IRepository where T : IAggregateRoot { }
}