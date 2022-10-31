using Architecture;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace Project.Infrastructure
{
    public class EventMediator : IEventMediator
    {
        private readonly IMediator _mediator;
        private readonly ProjectDbContext _dbContext;
        private readonly IIntegrationEventOutbox<IDbContextTransaction> _integrationEventOutbox;

        public EventMediator(IMediator mediator, ProjectDbContext dbContext, IIntegrationEventOutbox<IDbContextTransaction> integrationEventOutbox)
        {
            _mediator = mediator;
            _dbContext = dbContext;
            _integrationEventOutbox = integrationEventOutbox;
        }

        public Task PublishCommandAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(command, cancellationToken);
        }

        public Task<R> PublishCommandAsync<R>(ICommand<R> command, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(command, cancellationToken);
        }

        public Task PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent
        {
            return _mediator.Publish(domainEvent, cancellationToken);
        }

        public Task PublishIntegrationEventAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IntegrationEvent
        {
            return _integrationEventOutbox.AddEventAsync(_dbContext.CurrentTransaction, integrationEvent, cancellationToken);
        }

        public Task<R> PublishQueryAsync<R>(IQuery<R> query, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(query, cancellationToken);
        }
    }
}