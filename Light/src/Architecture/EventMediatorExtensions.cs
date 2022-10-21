namespace Architecture
{
    public static class EventMediatorExtensions
    {
        public static async Task PublishDomainEventsAsync(this IEventMediator mediator, IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
        {
            foreach (var domainEvent in domainEvents)
            {
                await mediator.PublishDomainEventAsync(domainEvent, cancellationToken);
            }
        }
    }
}