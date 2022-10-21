namespace Architecture
{
    public interface IInetgrationEventPublisher
    {
        Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken);
    }
}