using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;

namespace Project.UseCase.Test
{
    public abstract class FakeEventMediator : IEventMediator
    {
        public List<IBaseCommand> Commands = new List<IBaseCommand>();
        public List<IDomainEvent> DomainEvents = new List<IDomainEvent>();
        public List<IntegrationEvent> IntegrationEvents = new List<IntegrationEvent>();

        public abstract Task PublishCommandAsync(ICommand command, CancellationToken cancellationToken = default);
        public abstract Task<R> PublishCommandAsync<R>(ICommand<R> command, CancellationToken cancellationToken = default);
        public abstract Task PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
        public abstract Task PublishIntegrationEventAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IntegrationEvent;
        public abstract Task PublishIntegrationEventsAsync<T>(IEnumerable<T> integrationEvents, CancellationToken cancellationToken = default) where T : IntegrationEvent;
        public abstract Task<R> PublishQueryAsync<R>(IQuery<R> query, CancellationToken cancellationToken = default);
    }
}