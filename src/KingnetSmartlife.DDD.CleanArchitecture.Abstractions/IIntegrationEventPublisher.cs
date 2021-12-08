using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IIntegrationEventPublisher
    {
        Task SaveEventAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken);
        Task SaveEventsAsync(IEnumerable<IntegrationEvent> integrationEvents, CancellationToken cancellationToken);
        Task PublishEventsAsync(CancellationToken cancellationToken);
        Task PublishTimeoutEventsAsync(CancellationToken cancellationToken);
    }
}