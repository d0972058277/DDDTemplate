using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
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