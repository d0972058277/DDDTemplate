namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IInetgrationEventPublisher
    {
        Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken);
    }
}