using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KingnetSmartlife.DDD.CleanArchitecture.Abstractions
{
    public interface IEventMediator
    {
        Task PublishCommandAsync(ICommand command, CancellationToken cancellationToken = default);
        Task<R> PublishCommandAsync<R>(ICommand<R> command, CancellationToken cancellationToken = default);
        Task<R> PublishQueryAsync<R>(IQuery<R> query, CancellationToken cancellationToken = default);
        Task PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
        Task PublishIntegrationEventAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IntegrationEvent;
        Task PublishIntegrationEventsAsync<T>(IEnumerable<T> integrationEvents, CancellationToken cancellationToken = default) where T : IntegrationEvent;
    }
}