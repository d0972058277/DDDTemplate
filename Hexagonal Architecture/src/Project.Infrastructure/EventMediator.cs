using Architecture;
using MediatR;

namespace Project.Infrastructure
{
    public class EventMediator : IEventMediator
    {
        private readonly IMediator _mediator;
        private readonly ProjectDbContext _dbContext;

        public EventMediator(IMediator mediator, ProjectDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
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
            throw new NotImplementedException();
        }

        public Task<R> PublishQueryAsync<R>(IQuery<R> query, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(query, cancellationToken);
        }
    }
}