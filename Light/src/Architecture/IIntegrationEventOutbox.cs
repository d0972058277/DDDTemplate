namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IIntegrationEventOutbox
    {
        Task PublishTimeoutEventsAsync(CancellationToken cancellationToken);
    }

    public interface IIntegrationEventOutbox<T> : IIntegrationEventOutbox
    {
        Task AddEventAsync(T transactionId, IntegrationEvent integrationEvent, CancellationToken cancellationToken);
        Task PublishEventsAsync(T transactionId, CancellationToken cancellationToken);
    }
}