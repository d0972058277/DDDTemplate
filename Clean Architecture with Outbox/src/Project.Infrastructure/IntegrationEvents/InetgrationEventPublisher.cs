using Architecture;
using MassTransit;

namespace Project.Infrastructure.IntegrationEvents
{
    public class InetgrationEventPublisher : IInetgrationEventPublisher
    {
        private readonly IntegrationEventCorrelationService _correlationService;
        private readonly IPublishEndpoint _publishEndpoint;

        public InetgrationEventPublisher(IntegrationEventCorrelationService correlationService, IPublishEndpoint publishEndpoint)
        {
            _correlationService = correlationService;
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            return _publishEndpoint.Publish(integrationEvent as object, ctx => ctx.CorrelationId = _correlationService.CorrelationId, cancellationToken);
        }
    }
}